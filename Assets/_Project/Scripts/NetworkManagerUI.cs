using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;
    [SerializeField] GameObject _uiHolder;

    void Awake()
    {
        _hostButton.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });
        _clientButton.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });
       
    }

    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += Singleton_ServerStarted;
        NetworkManager.Singleton.OnClientStarted += Singleton_ServerStarted;
    }

    void Singleton_ServerStarted()
    {
        _uiHolder.SetActive(false);
    }
}
