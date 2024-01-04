using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _singlePlayerButton;
    [SerializeField] Button _multiplayerButton;
    [SerializeField] Button _quitButton;
    [SerializeField] CanvasGroup _mainMenuCanvasGroup;

    public enum MenuTypes
    {
        MainMenu,
        MultiplayerMenu,
        SinglePlayerMenu
    }

    //TODO If multiplayer menu button is pressed, load lobby scene?
    //This would give a new scene where it's connected to steam online and everything and would allow character selection fairly easily

    void Awake()
    {
        _singlePlayerButton.onClick.AddListener(() => { });
        _multiplayerButton.onClick.AddListener(() =>
        {
            LoadingSceneManager.Instance.LoadScene(LoadingSceneManager.SceneName.MultiPlayerLobby, false);
        });
        _quitButton.onClick.AddListener(() => { Application.Quit(); });

        StartCoroutine(FadeInMenu());
    }

    IEnumerator FadeInMenu()
    {
        yield return new WaitForSeconds(2.0f);
        UpdateVisibility(true);
    }

    void UpdateVisibility(bool visible)
    {
        switch (visible)
        {
            case true:
                _mainMenuCanvasGroup.alpha = 1;
                _mainMenuCanvasGroup.interactable = true;
                _mainMenuCanvasGroup.blocksRaycasts = true;
                break;
            case false:
                _mainMenuCanvasGroup.alpha = 0;
                _mainMenuCanvasGroup.interactable = false;
                _mainMenuCanvasGroup.blocksRaycasts = false;
                break;
        }
    }
}