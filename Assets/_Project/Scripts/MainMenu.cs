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
        _singlePlayerButton.onClick.AddListener(() =>
        {
            
        });
        _multiplayerButton.onClick.AddListener(() =>
        {
            
        });
        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

   

   
}