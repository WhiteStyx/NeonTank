using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;

public class LobbyButton : MonoBehaviour
{
    public Lobby hostLobby, joinedLobby;
    float heartbeatTimer, lobbyUpdateTimer;
    LobbyManager thescript;
    [SerializeField] GameObject testLobby;
    string playerName;

    private async void Update()
    {
        await HandleLobbyHeartbeat();
        await HandleLobbyPollUpdate();
    }

    public async void _CreateLobby()
    {
        playerName = testLobby.GetComponent<LobbyManager>().playerName;
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

    private async Task CreateLobby()
    {
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
        try{
        QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
            Count = 25,
            Filters = new List<QueryFilter>{
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            },
            Order = new List<QueryOrder>{
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            }
        };

        QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

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

    private async Task QuickJoinLobby()
    {
        try{
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            joinedLobby = lobby;

            Debug.Log("Joined Lobby with code" + lobby.LobbyCode);
            PrintPlayer(lobby);
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
            float heartbeatTimerMax = 15f;
            heartbeatTimer = heartbeatTimerMax;
            Debug.Log("kinda");
            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        }
    }
    
}
