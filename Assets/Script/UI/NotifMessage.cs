using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies;

public class NotifMessage : MonoBehaviour
{
    TMP_Text objectText;

    void Start()
    {
        objectText = GetComponentInChildren<TMP_Text>();
    }

    public void PopUpMessage(string text, LobbyServiceException e)
    {
        objectText.text = text;
    }
}
