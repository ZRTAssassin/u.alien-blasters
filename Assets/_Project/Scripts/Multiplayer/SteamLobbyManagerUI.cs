using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using Steamworks.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Image = Steamworks.Data.Image;

public class SteamLobbyManagerUI : MonoBehaviour
{
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;
    [SerializeField] GameObject _playerListHolder;
    [SerializeField] GameObject _playerDisplayPrefab;
    [SerializeField] CanvasGroup _networkManagerUI;
    [SerializeField] List<GameObject> _lobbyMembers = new();

    void Awake()
    {
        UpdateVisibility(false);
        //
    }

    void Start()
    {
        _clientButton.onClick.AddListener(OnClientClicked);
        //TODO maybe we need to tell the game to create a lobby *after* you start the game?
        _hostButton.onClick.AddListener(() => { SteamLobbyManager.Instance.CreateLobby(); });
        // when clicking the button you need to start host in the game, and then start a steam lobby where you are also a host.
        // then when you join someone, you join the lobby and also start the client.

        SteamLobbyManager.Instance.OnFriendJoin += SteamLobbyManagerOnFriendJoin;
        SteamLobbyManager.Instance.OnFriendLeave += SteamLobbyManagerOnFriendLeave;
        SteamLobbyManager.Instance.OnSteamLobbyCreated += SteamLobbyManagerOnLobbyCreated;
        
        LoadingSceneManager.Instance.SceneChanged += HandleSceneChanged;
    }
    void HandleSceneChanged(LoadingSceneManager.SceneName obj)
    {
        if (obj == LoadingSceneManager.SceneName.MultiPlayerLobby)
        {
            StartCoroutine(FadeInLobbyControls());
        }
    }
    IEnumerator FadeInLobbyControls()
    {
        yield return new WaitForSeconds(2.0f);
        UpdateVisibility(true);
    }

    void OnClientClicked()
    {
        Debug.Log("SteamLobbyManagerUI - OnClientClicked: ");
        SteamLobbyManager.Instance.StartGame();
    }
    void SteamLobbyManagerOnLobbyCreated(Lobby obj)
    {
        while (_playerListHolder.transform.childCount > 0)
        {
            DestroyImmediate(_playerListHolder.transform.GetChild(0).gameObject);
        }
    }

    async void SteamLobbyManagerOnFriendLeave(Friend friend)
    {
        //todo, just read the list of players and redo display? Or give them an ID or something to use easily OH WAIT
        foreach (var player in SteamLobbyManager.CurrentPlayers)
        {
            GameObject newPlayerGo = Instantiate(_playerDisplayPrefab, _playerListHolder.transform);
            SteamProfileDisplay steamProfileDisplay = newPlayerGo.GetComponent<SteamProfileDisplay>();
            steamProfileDisplay.SetProfileName(player.Name);
            steamProfileDisplay.SetSteamId(player.Id);
            Image? newImg = await SteamFriends.GetLargeAvatarAsync(player.Id);

            if (newImg is not null)
            {
                var texture = Texture2DHelpers.Covert((Image)newImg);
                steamProfileDisplay.SetProfilePicture(texture);
            }
        }
        Debug.Log($"SteamNetworkManagerOnOnFriendJoin called. {friend.Name} left");
    }

    async void SteamLobbyManagerOnFriendJoin(Friend friend)
    {
        foreach (var player in SteamLobbyManager.CurrentPlayers)
        {
            GameObject newPlayerGo = Instantiate(_playerDisplayPrefab, _playerListHolder.transform);
            SteamProfileDisplay steamProfileDisplay = newPlayerGo.GetComponent<SteamProfileDisplay>();
            steamProfileDisplay.SetProfileName(player.Name);
            steamProfileDisplay.SetSteamId(player.Id);
            Image? newImg = await SteamFriends.GetLargeAvatarAsync(player.Id);

            if (newImg is not null)
            {
                var texture = Texture2DHelpers.Covert((Image)newImg);
                steamProfileDisplay.SetProfilePicture(texture);
            }
        }
        Debug.Log($"SteamNetworkManagerOnOnFriendJoin called. {friend.Name} joined");
        
        
    }

    void OnApplicationQuit()
    {
        if (SteamLobbyManager.Instance == null) return;
        SteamLobbyManager.Instance.OnFriendJoin -= SteamLobbyManagerOnFriendJoin;
        SteamLobbyManager.Instance.OnFriendLeave -= SteamLobbyManagerOnFriendLeave;
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