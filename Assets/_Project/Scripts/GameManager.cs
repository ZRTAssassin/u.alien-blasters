using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action<Gamestate> OnGamestateChanged;

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

    void Start()
    {
        UpdateGameState(Gamestate.Initializing);
    }

    public void UpdateGameState(Gamestate newState)
    {
        OnGamestateChanged?.Invoke(newState);

        switch (newState)
        {
            case Gamestate.Initializing:
                HandleInitializing();
                break;
            case Gamestate.Menu:
                HandleMenu();
                break;
            case Gamestate.Loading:
                break;
            case Gamestate.Level:
                break;
            case Gamestate.Lobby:
                HandleLobby();
                break;
            case Gamestate.Playing:
                break;
            default:
                Debug.LogError($"{this.name} UpdateGameState{newState}");
                break;
        }
    }

    void HandleLobby()
    {
        MenuManager.Instance.UpdateMenuVisibility(MenuManager.MenuTypes.MultiplayerMenu, true);
    }

    void HandleMenu()
    {
        MenuManager.Instance.UpdateMenuVisibility(MenuManager.MenuTypes.MainMenu, true);
    }

    async void HandleInitializing()
    {
        Debug.Log($"{name} HandleInitializing");

        await Task.Delay(3000);
        UpdateGameState(Gamestate.Menu);
    }

    public enum Gamestate
    {
        Initializing,
        Menu,
        Loading,
        Level,
        Lobby,
        Playing
    }
}