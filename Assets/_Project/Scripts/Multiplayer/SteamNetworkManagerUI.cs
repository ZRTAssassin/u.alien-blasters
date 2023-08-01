using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamNetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;
    [SerializeField] GameObject _uiHolder;
    [SerializeField] SteamNetworkManager _manager;


    void Start()
    {
        _manager = GetComponent<SteamNetworkManager>();
        _hostButton.onClick.AddListener(() => { _manager.StartHost(); });
        _clientButton.onClick.AddListener(() => { _manager.StartGame(); });
    }
}