using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;
    [SerializeField] GameObject _uiHolder;
    [SerializeField] TMP_InputField _joinCodeInput;
    [SerializeField] TMP_Text _joinCodeDisplay;

    void Awake()
    {
        _hostButton.onClick.AddListener(() =>
        {
            CreateRelay();
        });
        _clientButton.onClick.AddListener(() =>
        {
            //Let's add a box to get the join code with so we can put it in
            var code = _joinCodeInput.text;
            if (string.IsNullOrEmpty(code)) return;
            JoinRelay(code);
        });
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
        
        // turn on some kind of spinner

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        NetworkManager.Singleton.OnServerStarted += Singleton_ServerStarted;
        NetworkManager.Singleton.OnClientStarted += Singleton_ServerStarted;
    }
[ContextMenu("Relay/Create Relay")]
    async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
                );
            NetworkManager.Singleton.StartHost();
            if (_joinCodeDisplay != null)
            {
                _joinCodeDisplay.text = $"{joinCode}";
            }
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    [ContextMenu("Relay/Join Relay")]
    async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );
            Debug.Log(joinCode);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    void Singleton_ServerStarted()
    {
        //Debug.Log($"{_transport.ConnectionData.Address}, {_transport.ConnectionData.Port}, {_transport.ConnectionData.ServerListenAddress}, {_transport.ConnectionData.ListenEndPoint}, {_transport.ConnectionData.ServerEndPoint}");
        _uiHolder.SetActive(false);
    }
}