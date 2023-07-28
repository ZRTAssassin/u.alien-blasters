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
    [SerializeField] Button _AutoIPSetup;

    void Awake()
    {
        _transport = GetComponent<UnityTransport>();
        _setIPButton.onClick.AddListener(HandleSetIP);
        _AutoIPSetup.onClick.AddListener(HandleSetIPToDevIP);

        var thing2 = _transport.ConnectionData.ServerListenAddress;
        var thing = _transport.ConnectionData.ListenEndPoint;
        Debug.Log($"address: {thing2}, endpoint: {thing}");
    }

    void HandleSetIPToDevIP()
    {
        Debug.Log("HandleSetIPToDevIP");

        SetIPAddress("99.101.241.159", Convert.ToUInt16(30000));
    }

    void Start()
    {
        //_ipText.text = $"{_transport.ConnectionData.Address}:{_transport.ConnectionData.Port}";
    }

    void HandleSetIP()
    {
        Debug.Log("HandleSetIP");
        if (IsValidIP(_ipInput.text) && IsValidPort(_portInput.text))
        {
            SetIPAddress(_ipInput.text, Convert.ToUInt16(_portInput.text));
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
        Debug.Log($"SetIPAddress {newIP}: {newPort}");
        _transport.ConnectionData.Address = $"{newIP}";
        _transport.ConnectionData.Port = newPort;
        _ipText.text = $"{newIP}: {newPort}";
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