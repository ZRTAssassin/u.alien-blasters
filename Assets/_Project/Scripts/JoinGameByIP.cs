using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class JoinGameByIP : MonoBehaviour
{
    [SerializeField] UnityTransport _transport;
    [SerializeField] Button _setIPButton;
    [SerializeField] TMP_InputField _ipInput;
    [SerializeField] TMP_InputField _portInput;
    [SerializeField] TMP_Text _ipText;

    void Awake()
    {
        _transport = GetComponent<UnityTransport>();
        _setIPButton.onClick.AddListener(HandleSetIP);
    }

    void Start()
    {
        _ipText.text = $"{_transport.ConnectionData.Address}:{_transport.ConnectionData.Port}";
    }

    void HandleSetIP()
    {
        Debug.Log("Button Clicked bru");
        if (IsValidIP(_ipInput.text) && IsValidPort(_portInput.text))
        {
            SetIPAddress(_ipInput.text, Convert.ToUInt16(_portInput.text));
            _ipText.text = $"{_ipInput.text}: {_portInput.text}";
        }
        
    }

    [ContextMenu("Custom Functions/Set IP")]
    void SetIPAddress()
    {
        string newIP = "192.168.0.1";
        ushort newPort = 1234;
        _transport.ConnectionData.Address = $"{newIP}";
        _transport.ConnectionData.Port = newPort;
    }

    void SetIPAddress(string newIP, ushort newPort)
    {
        _transport.ConnectionData.Address = $"{newIP}";
        _transport.ConnectionData.Port = newPort;
    }


    bool IsValidIP(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return false;
        }

        string[] splitValues = ip.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        byte tempForParsing;
    
        return splitValues.All(r => byte.TryParse(r, out tempForParsing));
    }

    bool IsValidPort(string port)
    {
        int number;
        if (Int32.TryParse(port, out number))
        {
            return number >= 0 && number <= 65535;
        }

        // Couldn't convert string to integer, so it's not a valid port.
        return false;
    }
}