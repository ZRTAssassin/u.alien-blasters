using System;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;

public class SteamNetworkManager : MonoBehaviour
{
    public static SteamNetworkManager Instance { get; private set; }
    public Lobby? CurrentLobby { get; private set; } = null;
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

        Debug.Log($"Joining room hosted by {_transport.targetSteamId}", this);

        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client has joined", this);
        }
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
        Debug.Log("Joining a lobby now.");
        CurrentLobby = lobby;
        Debug.Log($"Owner: {lobby.Owner}, Id: {id}, IsSame: {isSame}", this);
        CurrentLobby?.Join();
        //StartClient(lobby.Id);
    }

    void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id)
    {
    }

    void OnLobbyInvite(Friend friend, Lobby lobby)
    {
        Debug.Log($"You got an invite from {friend.Name}", this);
    }

    void OnLobbyMemberLeave(Lobby lobby, Friend friend)
    {
    }

    void OnLobbyMemberJoined(Lobby lobby, Friend friend)
    {
    }

    void OnLobbyEntered(Lobby lobby)
    {
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
        Debug.Log($"Lobby has been created, {lobby.Id}", this);
    }

    #endregion
}