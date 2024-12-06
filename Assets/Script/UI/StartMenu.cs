using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MenuBase
{
    Button hostButton, joinButton, backButton;
    void Start()
    {
        hostButton = transform.Find("Host").GetComponent<Button>();
        joinButton = transform.Find("Join").GetComponent<Button>();
        backButton = transform.Find("Back").GetComponent<Button>();

        hostButton.onClick.AddListener(() =>
        {
            Host();
        });
        joinButton.onClick.AddListener(() =>
        {
            Join();
        });
        backButton.onClick.AddListener(() => Back());
    }

    private void Host()
    {
        loadingScene.SetActive(true);
        gameObject.SetActive(false);
        LobbyManager.Instance.CreateLobby();
    }

    private void Join()
    {
        loadingScene.SetActive(true);
        gameObject.SetActive(false);
        lobbyMenu.SetActive(true);
        LobbyManager.Instance.ListLobbies();
        loadingScene.SetActive(false);
    }

    private void Back()
    {
        loadingScene.SetActive(true);
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
        loadingScene.SetActive(false);
    }
}
