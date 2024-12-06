using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuBase
{
    Button startButton, settingButton, exitButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton = transform.Find("Start").GetComponent<Button>();
        settingButton = transform.Find("Setting").GetComponent<Button>();
        exitButton = transform.Find("Exit").GetComponent<Button>();

        startButton.onClick.AddListener(() => StartMenu());
        settingButton.onClick.AddListener(() => SettingMenu());
        exitButton.onClick.AddListener(() => Exit());
    }

    void StartMenu()
    {
        gameObject.SetActive(false);
        startMenu.SetActive(true);
    }

    void SettingMenu()
    {
        gameObject.SetActive(false);
        inputMenu.SetActive(true);
    }

    void Exit()
    {
        Application.Quit();
    }
}
