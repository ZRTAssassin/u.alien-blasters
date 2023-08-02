using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Image = Steamworks.Data.Image;

public class SteamNetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;
    [SerializeField] GameObject _playerListHolder;
    [SerializeField] GameObject _playerDisplayPrefab;
    [SerializeField] CanvasGroup _networkManagerUI;


    void Start()
    {
        _hostButton.onClick.AddListener(() => { SteamNetworkManager.Instance.StartHost(); });
        _clientButton.onClick.AddListener(() => { SteamNetworkManager.Instance.StartGame(); });
        SteamNetworkManager.Instance.OnFriendJoin += SteamNetworkManagerOnOnFriendJoin;
        SteamNetworkManager.Instance.OnFriendLeave += SteamNetworkManagerOnOnFriendLeave;
    }

    void SteamNetworkManagerOnOnFriendLeave(Friend friend)
    {
        //todo, just read the list of players and redo display? Or give them an ID or something to use easily OH WAIT
    }

    async void SteamNetworkManagerOnOnFriendJoin(Friend friend)
    {
        Debug.Log($"SteamNetworkManagerOnOnFriendJoin called. {friend.Name} joined");
        GameObject newPlayerGo = Instantiate(_playerDisplayPrefab, _playerListHolder.transform);
        SteamProfileDisplay steamProfileDisplay = newPlayerGo.GetComponent<SteamProfileDisplay>();
        steamProfileDisplay.SetProfileName(friend.Name);
        Image? newImg = await SteamFriends.GetLargeAvatarAsync(friend.Id);

        if (newImg is not null)
        {
            var texture = Texture2DHelpers.Covert((Image)newImg);
            steamProfileDisplay.SetProfilePicture(texture);
        }


    }

    void OnApplicationQuit()
    {
        if (SteamNetworkManager.Instance == null) return;
        SteamNetworkManager.Instance.OnFriendJoin -= SteamNetworkManagerOnOnFriendJoin;
        SteamNetworkManager.Instance.OnFriendLeave -= SteamNetworkManagerOnOnFriendLeave;
    }

    public void UpdateVisibility(bool visible)
    {
        switch (visible)
        {
            case true:
                _networkManagerUI.alpha = 1;
                _networkManagerUI.interactable = true;
                _networkManagerUI.blocksRaycasts = true;
                break;
            case false:
                _networkManagerUI.alpha = 0;
                _networkManagerUI.interactable = false;
                _networkManagerUI.blocksRaycasts = false;
                break;

        }
    }
    
}