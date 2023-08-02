using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.LevelManagement;
using UnityEngine;

public class GamesListPanel : MonoBehaviour
{
    [SerializeField] LoadGameButton _buttonPrefab;

    void Start()
    {
        foreach (var gameName in GameManager_Old.Instance.AllGameNames)
        {
            var button = Instantiate(_buttonPrefab, transform);
            button.SetGameName(gameName);
        }
    }
}
