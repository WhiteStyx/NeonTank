using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputWindow : UI_ButtonBase
{
    Button joinButton;
    Button cancelButton;
    TMP_InputField inputField;

    private void Awake()
    {
        joinButton = transform.Find("Join").GetComponent<Button>();
        cancelButton = transform.Find("Cancel").GetComponent<Button>();
        inputField = transform.Find("InputField").GetComponent<TMP_InputField>();
    }
}
