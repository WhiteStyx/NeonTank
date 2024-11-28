using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButtonUI : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] Button quickJoinButton;
    [SerializeField] Button refreshButton;

    void Awake()
    {
        hostButton = transform.Find("HostButton").GetComponent<Button>();
        quickJoinButton = transform.Find("QuickJoin").GetComponent<Button>();
        refreshButton = transform.Find("Refresh").GetComponent<Button>();

        hostButton.onClick.AddListener(() => 
        {
            LobbyManager.Instance.CreateLobby();
        });

        quickJoinButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.QuickJoinLobby();
        });

        refreshButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.ListLobbies();
        });
    }
}
