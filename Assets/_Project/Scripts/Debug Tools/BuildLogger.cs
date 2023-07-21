using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildLogger : MonoBehaviour
{
    [SerializeField] bool _log;
    [Space(10)] [SerializeField] int maxLines = 50;
    [SerializeField] TextMeshProUGUI _debugLogText;

    Queue<string> queue = new Queue<string>();

    void Awake()
    {
        if (!_log)
        {
            _debugLogText.text = string.Empty;
        }
    }

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
        if (!_log) return;
        // Delete oldest message
        if (queue.Count >= maxLines) queue.Dequeue();

        queue.Enqueue(logString);

        var builder = new StringBuilder();
        foreach (string st in queue)
        {
            builder.Append(st).Append("\n");
        }

        _debugLogText.text = builder.ToString();
    }
}