using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    void Start()
    {
        GameManager.Instance.OnGamestateChanged += HandleOnGamestateChanged;
    }

    void HandleOnGamestateChanged(GameManager.Gamestate newState)
    {
        _text.text = newState.ToString();
    }
}
