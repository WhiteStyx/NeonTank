using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class OnScreenConsoleLog : MonoBehaviour
{
    [SerializeField] int maxLine;
    [SerializeField] TextMeshProUGUI debugLogText;
    Queue<string> queue = new Queue<string>();
    Vector2 scrollPosition;
    string logText;

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if(queue.Count >= maxLine) queue.Dequeue();
        
        queue.Enqueue("[ " + type + " ]" + logString);
        var builder = new StringBuilder();
        foreach (string st in queue)
        {
            builder.Append(st).Append("\n");
        }

        debugLogText.text = builder.ToString();
    }

    /*
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height/2));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width - 20), GUILayout.Height(Screen.height / 2));
        GUILayout.Label(logText);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
    */

}
