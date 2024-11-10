using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using TMPro;

public class TestLobby : MonoBehaviour
{
    float heartbeatTimer = 15f;
    float heartbeatTimerMax = 15f;
    float lobbyUpdateTimer;
    LobbyButton lobbyButton;
    TMP_InputField codeToJoin;
    public Lobby hostLobby, joinedLobby;
    public bool create, check, join, joinCode, pPlayer, update;
    public string playerName;
    [SerializeField] private GameObject inputWindow;
    [SerializeField] string lobbyCode;

    private async void Start()
    {
        await Initialize();
    }

    private async void Update()
    {
        if(create)  await CreateLobby();
        if(check)   await ListLobbies();
        if(join)    await JoinLobby();
        if(joinCode) await JoinLobbyByCode(lobbyCode);
        if(pPlayer) PrintPlayer();
        await HandleLobbyHeartbeat();
        await HandleLobbyPollUpdate();
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

    #region UpdateLobby
    private async Task HandleLobbyHeartbeat()
    {
        if(hostLobby == null)
        {
            // Debug.Log("null");
            return;
        }
        heartbeatTimer -= Time.deltaTime;
        if(heartbeatTimer < 0f)
        {
            heartbeatTimer = heartbeatTimerMax;
            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        }
    }

    private async Task HandleLobbyPollUpdate()
    {
        if(joinedLobby == null)
        {
            // Debug.Log("null");
            return;
        }
        lobbyUpdateTimer -= Time.deltaTime;
        if(lobbyUpdateTimer < 0f)
        {
            float lobbyUpdateTimerMax = 1.1f;
            lobbyUpdateTimer = lobbyUpdateTimerMax;

            Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
            joinedLobby = lobby;
        }
    }
    #endregion

    private async Task CreateLobby()
    {
        create = false;
        try
        {
        string lobbyName = "My Lobby";
        int maxPlayer = 4;
        CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
            IsPrivate = false,
            Player = GetPlayer(),
            Data = new Dictionary<string, DataObject>{
                {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag", DataObject.IndexOptions.S1)}
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, createLobbyOptions);

        hostLobby = lobby;
        joinedLobby = hostLobby;
        
        Debug.Log("Created Lobby " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.LobbyCode);
        PrintPlayer(hostLobby);
        } 
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task ListLobbies()
    {
        check = false;
        try{
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Count = 25,
                Filters = new List<QueryFilter>{new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)},
                Order = new List<QueryOrder>{new QueryOrder(false, QueryOrder.FieldOptions.Created)}
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found : " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task JoinLobbyByCode(string lobbyCode)
    {
        joinCode = false;
        try{
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;

            Debug.Log("Joined Lobby with code : " + lobbyCode);
            PrintPlayer(lobby);
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task JoinLobby()
    {
        join = false;
        try{
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
            Debug.Log("Joined Lobby");
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task QuickJoinLobby()
    {
        try{
            Player player = GetPlayer();
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync(new QuickJoinLobbyOptions{
                Player = player
            });
            joinedLobby = lobby;

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
        await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private Player GetPlayer()
    {
        return new Player{
            Data = new Dictionary<string, PlayerDataObject> {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
            }
        };
    }

    public void PrintPlayer()
    {
        pPlayer = false;
        PrintPlayer(joinedLobby);
    }

    private void PrintPlayer(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name);
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    #region buttons
    public async void _CreateLobby()
    {
        await CreateLobby();
    }

    public async void _ListLobbies()
    {
        await ListLobbies();
    }

    public async void _QuickJoin()
    {
        await QuickJoinLobby();
    }

    #endregion
}
