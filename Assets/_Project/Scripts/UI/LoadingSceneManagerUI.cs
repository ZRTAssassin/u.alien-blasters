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
        LoadingSceneManager.Instance.SceneChanged += HandleSceneChanged;
        _text.text = LoadingSceneManager.Instance.SceneActive.ToString();
    }

    void OnDestroy()
    {
        
        LoadingSceneManager.Instance.SceneChanged -= HandleSceneChanged;
    }

    void HandleSceneChanged(LoadingSceneManager.SceneName sceneName)
    {
        _text.text = sceneName.ToString();
    }
}
