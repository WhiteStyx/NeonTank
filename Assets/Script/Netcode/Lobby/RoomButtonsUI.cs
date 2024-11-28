using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButtonsUI : MonoBehaviour
{
    Button startButton;
    void Awake()
    {
        startButton = transform.Find("Start").GetComponent<Button>();
        startButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.StartGame();
        });
    }
}
