using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListUI : MonoBehaviour
{
    [HideInInspector] public string playerName;
    TMP_Text playerNameUI;
    
    void Start()
    {
        playerNameUI = transform.Find("PlayerName").GetComponent<TMP_Text>();
        RefreshList();
    }

    public void RefreshList()
    {
        playerNameUI.text = playerName;
    }
}
