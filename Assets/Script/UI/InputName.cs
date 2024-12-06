using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InputName : MenuBase
{
    [SerializeField]TMP_InputField inputField;
    Button confirmButton, backButton;

    void Start()
    {
        confirmButton = transform.Find("Confirm").GetComponent<Button>();
        backButton = transform.Find("Back").GetComponent<Button>();
        inputField = gameObject.GetComponentInChildren<TMP_InputField>();

        confirmButton.onClick.AddListener(() => Confirm());
        backButton.onClick.AddListener(() => Back());
    }

    private void Confirm()
    {
        LobbyManager.Instance.playerName = inputField.text; 
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void Back()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
