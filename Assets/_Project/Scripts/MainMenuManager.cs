using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _mainMenu;
    [SerializeField] CanvasGroup _multiplayerMenu;
    [SerializeField] CanvasGroup _singlePlayerMenu;
    [SerializeField] Button _singlePlayerButton;
    [SerializeField] Button _multiplayerButton;
    [SerializeField] Button _quitButton;

    public enum MenuTypes
    {
        MainMenu,
        MultiplayerMenu,
        SinglePlayerMenu
    }
    
    //TODO If multiplayer menu button is pressed, load lobby scene?
    //This would give a new scene where it's connected to steam online and everything and would allow character selection fairly easily

    public static MainMenuManager Instance { get; private set; }

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

        _singlePlayerButton.onClick.AddListener(() => { Debug.Log("Singe Player Button Clicked"); });
        _multiplayerButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UpdateGameState(GameManager.Gamestate.Lobby);
        });
        _quitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    public void UpdateMenuVisibility(MenuTypes menuName)
    {
        switch (menuName)
        {
            case MenuTypes.MainMenu:
                UpdateVisibility(_mainMenu, true);
                UpdateVisibility(_multiplayerMenu, false);
                UpdateVisibility(_singlePlayerMenu, false);
                break;
            case MenuTypes.MultiplayerMenu:
                UpdateVisibility(_mainMenu, false);
                UpdateVisibility(_multiplayerMenu, true);
                UpdateVisibility(_singlePlayerMenu, false);
                break;
            case MenuTypes.SinglePlayerMenu:
                UpdateVisibility(_mainMenu, false);
                UpdateVisibility(_multiplayerMenu, false);
                UpdateVisibility(_singlePlayerMenu, true);
                break;
            default:
                Debug.Log($"{name} UpdateMenuVisibility called and got in a weid state. {menuName}");
                break;
        }
    }

    void UpdateVisibility(CanvasGroup canvasGroup, bool isVisible)
    {
        
        if (canvasGroup is null) return;
        if (isVisible)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}