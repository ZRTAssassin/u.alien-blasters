using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingSceneManagerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    void Start()
    {
        
    }

    void HandleOnGamestateChanged(LoadingSceneManager.SceneName newState)
    {
        _text.text = newState.ToString();
    }
}
