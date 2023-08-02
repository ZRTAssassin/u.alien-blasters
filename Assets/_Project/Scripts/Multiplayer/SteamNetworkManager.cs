using System;
using System.Collections.Generic;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;

public class SteamNetworkManager : MonoBehaviour
{
    public static SteamNetworkManager Instance { get; private set; }

    public event Action<Friend> OnFriendLeave;
    public event Action<Friend> OnFriendJoin;

    public Lobby? CurrentLobby { get; private set; } = null;
    public List<Friend> CurrentPlayers = new();
    FacepunchTransport _transport = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // todo: remove start client from joining lobby
    // after joining the lobby, then start client on joinedlobby callback?

    void Start()
    {
        _transport = NetworkManager.Singleton.GetComponent<FacepunchTransport>();
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;


        var playername = SteamClient.Name;
        var playersteamid = SteamClient.SteamId;
        Debug.Log($"{playername}, {playersteamid}");
    }

    void OnDestroy()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite -= OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated -= OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;
        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }

    void OnApplicationQuit() => Disconnect();

    public async void StartHost()
    {
        CurrentLobby = await SteamMatchmaking.CreateLobbyAsync(4);
        Debug.Log($"Started Host: {CurrentLobby.ToString()}");
    }

    public void StartGame()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

        NetworkManager.Singleton.StartHost();
    }

    void StartClient(SteamId id)
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

        _transport.targetSteamId = id;

        Debug.Log($"Joining room with id {_transport.targetSteamId}", this);

        // if (NetworkManager.Singleton.StartClient())
        // {
        //     Debug.Log("Client has joined", this);
        // }
    }

    public void Disconnect()
    {
        CurrentLobby?.Leave();
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.Shutdown();
    }

    #region UnityCallbacks

    void OnServerStarted() => Debug.Log("Server has started", this);

    void OnClientConnectedCallback(ulong clientId) => Debug.Log($"Client connected, clientId={clientId}");

    void OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log($"Client disconnected, clientId={clientId}");
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }

    #endregion

    #region Steam Callbacks

    void OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
    {
        bool isSame = lobby.Owner.Id.Equals(id);
        //Debug.Log("Joining a lobby now.");
        CurrentLobby = lobby;
        Debug.Log($"OnGameLobbyJoinRequested: Owner: {lobby.Owner}, Id: {id}, IsSame: {isSame}", this);
        CurrentLobby?.Join();
        //StartClient(lobby.Id);
    }

    void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id)
    {
        Debug.Log($"OnLobbyGameCreated: {lobby.Owner.Name} created a lobby");
    }

    void OnLobbyInvite(Friend friend, Lobby lobby)
    {
        Debug.Log($"OnLobbyInvite: You got an invite from {friend.Name}", this);
    }

    void OnLobbyMemberLeave(Lobby lobby, Friend friend)
    {
        Debug.Log($"OnLobbyMemberLeave: {friend.Name} left");
        RemovePlayerFromPlayerList(friend);
    }

    void OnLobbyMemberJoined(Lobby lobby, Friend friend)
    {
        Debug.Log($"OnLobbyMemberJoined: {friend.Name} joined");
        AddPlayerToPlayerList(friend);
    }

    void OnLobbyEntered(Lobby lobby)
    {
        Debug.Log($"OnLobbyEntered: Joined {lobby.Owner.Name}'s lobby");
        foreach (Friend lobbyMember in lobby.Members)
        {
            AddPlayerToPlayerList(lobbyMember);
        }

        //unity call
        if (NetworkManager.Singleton.IsHost) return;
        StartClient(lobby.Id);
    }

    void OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result != Result.OK)
        {
            Debug.LogError($"Lobby couldn't be created, {result}", this);
            return;
        }

        lobby.SetFriendsOnly();
        lobby.SetData("name", "Cool Lobby Name");
        lobby.SetJoinable(true);
        CurrentLobby = lobby;

        Debug.Log($"OnLobbyCreated by {lobby.Id}", this);
    }

    #endregion


    void AddPlayerToPlayerList(Friend friend)
    {
        CurrentPlayers.Add(friend);
        OnFriendJoin?.Invoke(friend);
    }

    void RemovePlayerFromPlayerList(Friend friend)
    {
        CurrentPlayers.Remove(friend);
        OnFriendLeave?.Invoke(friend);
    }
}