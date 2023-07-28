using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;
    [SerializeField] GameObject _uiHolder;
    [SerializeField] UnityTransport _transport;

    void Awake()
    {
        _hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        _clientButton.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });
        _transport = GetComponent<UnityTransport>();
    }

    void Start()
    {
        
        NetworkManager.Singleton.OnServerStarted += Singleton_ServerStarted;
        NetworkManager.Singleton.OnClientStarted += Singleton_ServerStarted;
    }

    void Singleton_ServerStarted()
    {
        Debug.Log(
            $"{_transport.ConnectionData.Address}, {_transport.ConnectionData.Port}, {_transport.ConnectionData.ServerListenAddress}, {_transport.ConnectionData.ListenEndPoint}, {_transport.ConnectionData.ServerEndPoint}");
        _uiHolder.SetActive(false);
    }
}