using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoadingSceneManager : SingletonPersistent<LoadingSceneManager>
{
    public SceneName SceneActive => _sceneActive;
    SceneName _sceneActive;

    public void Init()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
    }


    public void LoadScene(SceneName sceneToLoad, bool isNetworkSessionActive = true)
    {
        StartCoroutine(Loading(sceneToLoad, isNetworkSessionActive));
    }

    IEnumerator Loading(SceneName sceneToLoad, bool isNetworkSessionActive)
    {
        
        LoadingFadeEffect.Instance.FadeIn();

        yield return new WaitUntil(() => LoadingFadeEffect._canLoad);
        
        if (isNetworkSessionActive)
        {
            if (NetworkManager.Singleton.IsServer)
                LoadSceneNetwork(sceneToLoad);
        }
        else
        {
            LoadSceneLocal(sceneToLoad);
        }

        // Because the scenes are not heavy we can just wait a second and continue with the fade.
        // In case the scene is heavy instead we should use additive loading to wait for the
        // scene to load before we continue
        yield return new WaitForSeconds(1f);

        LoadingFadeEffect.Instance.FadeOut();
    }

 // Load the scene using the regular SceneManager, use this if there's no active network session
    private void LoadSceneLocal(SceneName sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
        switch (sceneToLoad)
        {
            case SceneName.MainMenu:
                // if (AudioManager.Instance != null)
                //     AudioManager.Instance.PlayMusic(AudioManager.MusicName.intro);
                break;
        }
    }

    // Load the scene using the SceneManager from NetworkManager. Use this when there is an active
    // network session
    private void LoadSceneNetwork(SceneName sceneToLoad)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(
            sceneToLoad.ToString(),
            LoadSceneMode.Single);
    }

    // This callback function gets triggered when a scene is finished loading
    // Here we set up what to do for each scene, like changing the music
    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        // We only care the host/server is loading because every manager handles
        // their information and behavior on the server runtime
        if (!NetworkManager.Singleton.IsServer)
            return;

        Enum.TryParse(sceneName, out _sceneActive);

        // if (!ClientConnection.Instance.CanClientConnect(clientId))
        //     return;

        // What to initially do on every scene when it finishes loading
        switch (_sceneActive)
        {
            // When a client/host connects tell the manager
            case SceneName.CharacterSelection:
                //CharacterSelectionManager.Instance.ServerSceneInit(clientId);
                break;

            // When a client/host connects tell the manager to create the ship and change the music
            case SceneName.Gameplay:
                //GameplayManager.Instance.ServerSceneInit(clientId);
                break;

            // When a client/host connects tell the manager to create the player score ships and
            // play the right SFX
            //case SceneName.Victory:
            //case SceneName.Defeat:
                //EndGameManager.Instance.ServerSceneInit(clientId);
                //break;
        }
    }


    public enum SceneName
    {
        Bootstrap,
        MainMenu,
        SinglePlayerCharacterSelection,
        CharacterSelection,
        Controls,
        Gameplay,
        Playing
    }
}