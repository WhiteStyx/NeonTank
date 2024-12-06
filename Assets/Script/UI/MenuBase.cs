using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour
{
    [HideInInspector] public GameObject mainMenu, startMenu, lobbyMenu, roomMenu, inputMenu, loadingScene, canvas;

    void Awake()
    {
        canvas = transform.parent.gameObject;
        mainMenu = canvas.transform.Find("MainMenu").gameObject;
        startMenu = canvas.transform.Find("StartMenu").gameObject;
        lobbyMenu = canvas.transform.Find("Lobby").gameObject;
        roomMenu = canvas.transform.Find("InsideRoom").gameObject;
        inputMenu = canvas.transform.Find("InputMenu").gameObject;
        loadingScene = canvas.transform.Find("LoadingScene").gameObject;
    }
}
