using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class RoomButtonsUI : MenuBase
{
    Button startButton, leaveButton;
    void Start()
    {        
        startButton = transform.Find("Start").GetComponent<Button>();
        leaveButton = transform.Find("Leave").GetComponent<Button>();

        startButton.onClick.AddListener(() =>
        {
            loadingScene.SetActive(true);
            LobbyManager.Instance.StartGame();
        });
        leaveButton.onClick.AddListener(() =>
        {
            loadingScene.SetActive(true);
            LobbyManager.Instance.LeaveLobby();
            gameObject.SetActive(false);
            startMenu.SetActive(true);
            loadingScene.SetActive(false);
        });
    }
}
