using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class JoinListUI : MonoBehaviour
{
    [HideInInspector] public string lobbyName;
    [HideInInspector] public int maxPlayers;
    [HideInInspector] public int currentPlayers;
    [HideInInspector] public string lobbyId;
    [SerializeField] TMP_Text lobbyNameUI, playersUI, mapUI;
    Button button;

    void Start()
    {
        lobbyNameUI = transform.Find("LobbyName").GetComponent<TMP_Text>();
        playersUI = transform.Find("Players").GetComponent<TMP_Text>();
        mapUI = transform.Find("Map").GetComponent<TMP_Text>();
        
        button = gameObject.GetComponent<Button>();

        RefreshList();
        button.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobbyById(lobbyId);
        });
    }

    public void RefreshList()
    {
        lobbyNameUI.text = lobbyName;
        playersUI.text = currentPlayers.ToString() + "/" + maxPlayers.ToString();
        
    }

    public void JoinLobby()
    {
        LobbyManager.Instance.JoinLobbyById(lobbyId);
    }
}
