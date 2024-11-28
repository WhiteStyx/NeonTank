using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;

public class LobbyManager : MonoBehaviour
{
    float heartbeatTimer = 15f;
    float heartbeatTimerMax = 15f;
    float lobbyUpdateTimer;
    [SerializeField] GameObject joinLobbyPrefab;
    [SerializeField] GameObject inRoomPrefab;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject inputWindow;
    [SerializeField] GameObject loadingScreen;
    GameObject lobbyRoom, insideRoom;
    [SerializeField] RectTransform lobbyUI;
    RectTransform listUI;
    LobbyButton lobbyButton;
    TMP_InputField codeToJoin;
    public Lobby hostLobby;
    //public bool create, check, join, joinCode, pPlayer, update;
    public string playerName;
    [SerializeField] string lobbyCode;
    private static LobbyManager _instance;
    public static LobbyManager Instance
    {
        get
        {
            if(_instance == null) Debug.Log("Fuck");
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    async void Start()
    {
        lobbyRoom = canvas.transform.Find("Lobby").gameObject;
        insideRoom = canvas.transform.Find("InsideRoom").gameObject;
        loadingScreen = canvas.transform.Find("Loading").gameObject;
        await Initialize();
        loadingScreen.SetActive(false);
    }

    private void Update()
    {
        /*
        if(create)  await CreateLobby();
        if(check)   await ListLobbies();
        if(join)    await JoinLobby();
        if(joinCode) await JoinLobbyByCode(lobbyCode);
        if(pPlayer) PrintPlayer();
        */
        HandleLobbyHeartbeat();
        HandleLobbyPollUpdate();
        
    }

    private async Task Initialize()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Nameduh" + Random.Range(10, 99);
        Debug.Log(playerName);
    }

    async void HandleLobbyHeartbeat()
    {
        if(!IsLobbyHost()) return; 
        heartbeatTimer -= Time.deltaTime;
        if(heartbeatTimer < 0f)
        {
            heartbeatTimer = heartbeatTimerMax;
            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        }
    }

    async void HandleLobbyPollUpdate()
    {
        if(hostLobby == null)
        {
            // Debug.Log("null");
            return;
        }
        lobbyUpdateTimer -= Time.deltaTime;
        if(lobbyUpdateTimer < 0f)
        {
            float lobbyUpdateTimerMax = 1.1f;
            lobbyUpdateTimer = lobbyUpdateTimerMax;

            hostLobby = await LobbyService.Instance.GetLobbyAsync(hostLobby.Id);

            if(SceneManager.GetActiveScene().name != "Lobby") return;
            
            if(hostLobby.HostId != AuthenticationService.Instance.PlayerId)
            {
                if(hostLobby.Data["Started"].Value == "1")
                {
                    JoinGame();
                    return;
                }
            }

            if(hostLobby.Players[0].Data != null)
            {
                PrintPlayer(hostLobby);
            }
        }
    }

    public async void CreateLobby()
    {
        try
        {
            loadingScreen.SetActive(true);
            string lobbyName = "My Lobby";
            int maxPlayer = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>{
                    {"Started", new DataObject(DataObject.VisibilityOptions.Public, "0")},
                    {"Address", new DataObject(DataObject.VisibilityOptions.Public, "") },
                    {"Port", new DataObject(DataObject.VisibilityOptions.Public, "")},
                    {"Map", new DataObject(DataObject.VisibilityOptions.Public, "Circle", DataObject.IndexOptions.S1)}
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, createLobbyOptions);

            hostLobby = lobby;
            
            Debug.Log("Created Lobby " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.LobbyCode + " " + lobby.Id);
            lobbyRoom.SetActive(false);
            insideRoom.SetActive(true);
            loadingScreen.SetActive(false);
            PrintPlayer(hostLobby);
        } 
        catch(LobbyServiceException e)
        {
            loadingScreen.SetActive(false);
            Debug.Log(e);
        }
    }

    public async void ListLobbies()
    {
        try{
            listUI = canvas.transform.Find("Lobby/List").GetComponent<RectTransform>();
            Transform lobbyList = listUI.Find("Scroll View/Viewport/Content");
            for(int i = 0; i < lobbyList.childCount; i++)
            {
                Destroy(lobbyList.GetChild(i).gameObject);
            }
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found : " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
                JoinListUI joinListUI = Instantiate(joinLobbyPrefab, lobbyList).GetComponent<JoinListUI>();
                joinListUI.lobbyId = lobby.Id;
                joinListUI.lobbyName = lobby.Name;
                joinListUI.maxPlayers = lobby.MaxPlayers;
                joinListUI.currentPlayers = lobby.Players.Count;
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);

            }
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task JoinLobbyByCode(string lobbyCode)
    {
        try{
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            hostLobby = lobby;

            Debug.Log("Joined Lobby with code : " + lobbyCode);
            PrintPlayer(lobby);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyById(string lobbyId)
    {
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };

            hostLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, options);
            Debug.Log("Joined Lobby");
            lobbyRoom.SetActive(false);
            insideRoom.SetActive(true);
            PrintPlayer(hostLobby);
            
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoinLobby()
    {
        try{
            Player player = GetPlayer();
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync(new QuickJoinLobbyOptions{
                Player = player
            });
            hostLobby = lobby;

            Debug.Log("Joined Lobby with code" + lobby.LobbyCode);
            PrintPlayer(lobby);
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task LeaveLobby()
    {
        try{
        await LobbyService.Instance.RemovePlayerAsync(hostLobby.Id, AuthenticationService.Instance.PlayerId);
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public Player GetPlayer()
    {
        return new Player{
            Data = new Dictionary<string, PlayerDataObject> {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)},
                {"PlayerReady", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "0")}
            }
        };
    }

    private void PrintPlayer(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name + lobby.Players.Count);
        listUI = canvas.transform.Find("InsideRoom/List").GetComponent<RectTransform>();
        Transform lobbyList = listUI.Find("Scroll View/Viewport/Content");
        for(int i = 0; i < lobbyList.childCount; i++)
        {
            Destroy(lobbyList.GetChild(i).gameObject);
        }
        foreach(Player player in lobby.Players)
        {
            PlayerListUI playerList = Instantiate(inRoomPrefab, lobbyList).GetComponent<PlayerListUI>();
            playerList.playerName = player.Data["PlayerName"].Value;   

            
        }
    }

    public async void StartGame()
    {
        try
        {
            string address = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address;
            ushort port = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port;

            await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { "Started", new DataObject(DataObject.VisibilityOptions.Public, "1") },
                    { "Address", new DataObject(DataObject.VisibilityOptions.Public, address) },
                    { "Port", new DataObject(DataObject.VisibilityOptions.Public, port.ToString()) }
                }
            });
            //SceneManager.LoadScene("SceneOwen");
            NetworkManager.Singleton.StartHost();
            Loader.LoadNetwork("SceneOwen");
            
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void JoinGame()
    {
        try
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = hostLobby.Data["Address"].Value;
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(hostLobby.Data["Port"].Value);

            NetworkManager.Singleton.StartClient();
            Loader.LoadNetwork("SceneOwen");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private bool IsLobbyHost()
    {
        return hostLobby != null && hostLobby.HostId == AuthenticationService.Instance.PlayerId;
    }
}
