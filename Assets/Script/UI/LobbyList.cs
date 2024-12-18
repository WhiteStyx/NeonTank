using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyList : MenuBase
{
    Button backButton;
    void Start()
    {
        backButton = transform.Find("Back").GetComponent<Button>();
        
        backButton.onClick.AddListener(() => Back());
    }

    void Back()
    {
        gameObject.SetActive(false);
        startMenu.SetActive(true);
    }
}
