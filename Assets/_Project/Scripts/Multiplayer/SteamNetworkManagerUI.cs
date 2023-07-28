using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamNetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;
    [SerializeField] GameObject _uiHolder;


    void Awake()
    {
        _hostButton.onClick.AddListener(() =>
        { 
            SteamNetworkManager manager = GetComponent<SteamNetworkManager>();
            manager.StartHost();
        });
        _clientButton.onClick.AddListener(() =>
        {
            
        });
    }
}
