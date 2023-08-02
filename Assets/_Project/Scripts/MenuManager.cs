using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
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

    public static MenuManager Instance { get; private set; }

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

    public void UpdateMenuVisibility(MenuTypes menuName, bool isVisible)
    {
        switch (menuName)
        {
            case MenuTypes.MainMenu:
                UpdateVisibility(_mainMenu, isVisible);
                UpdateVisibility(_multiplayerMenu, !isVisible);
                UpdateVisibility(_singlePlayerMenu, !isVisible);
                break;
            case MenuTypes.MultiplayerMenu:
                UpdateVisibility(_mainMenu, !isVisible);
                UpdateVisibility(_multiplayerMenu, isVisible);
                UpdateVisibility(_singlePlayerMenu, !isVisible);
                break;
            case MenuTypes.SinglePlayerMenu:
                UpdateVisibility(_mainMenu, !isVisible);
                UpdateVisibility(_multiplayerMenu, !isVisible);
                UpdateVisibility(_singlePlayerMenu, isVisible);
                break;
            default:
                Debug.Log($"{name} UpdateMenuVisibility called and got in a weid state. {menuName}");
                break;
        }
    }

    void UpdateVisibility(CanvasGroup canvasGroup, bool isVisible)
    {
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