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
        //TODO maybe init steamworks inside of here?
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
        MainMenuManager.Instance.UpdateMenuVisibility(MainMenuManager.MenuTypes.MultiplayerMenu);
    }

    void HandleMenu()
    {
        MainMenuManager.Instance.UpdateMenuVisibility(MainMenuManager.MenuTypes.MainMenu);
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