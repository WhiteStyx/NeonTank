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
    [HideInInspector] public string hostName;
    [HideInInspector] public int maxPlayers;
    [HideInInspector] public int currentPlayers;
    [HideInInspector] public string lobbyId;
    [SerializeField] TMP_Text lobbyNameUI, playersUI, hostNameUI;
    Button button;

    void Start()
    {
        lobbyNameUI = transform.Find("LobbyName").GetComponent<TMP_Text>();
        playersUI = transform.Find("Players").GetComponent<TMP_Text>();
        hostNameUI = transform.Find("HostName").GetComponent<TMP_Text>();
        
        button = gameObject.GetComponent<Button>();

        RefreshList();
        button.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobbyById(lobbyId);
        });
    }

    public void RefreshList()
    {
        hostNameUI.text = hostName;
        playersUI.text = currentPlayers.ToString() + "/" + maxPlayers.ToString();
    }

    public void JoinLobby()
    {
        LobbyManager.Instance.JoinLobbyById(lobbyId);
    }
}
