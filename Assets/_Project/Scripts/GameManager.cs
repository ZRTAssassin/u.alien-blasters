using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<string> AllGameNames = new List<string>();


    [SerializeField] GameData _gameData;

    PlayerInputManager _playerInputManager;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += HandlePlayerJoined;

        SceneManager.sceneLoaded += HandleSceneLoaded;

        var commaNames = PlayerPrefs.GetString("AllGameNames");
        Debug.Log(commaNames);
        AllGameNames = commaNames.Split(",").ToList();
        AllGameNames.Remove("");
    }

    void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 0)
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        else
        {
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            SaveGame();
        }
    }

    void SaveGame()
    {
        string text = JsonUtility.ToJson(_gameData);
        Debug.Log(text);
        
        PlayerPrefs.SetString(_gameData.GameName, text);

        if (AllGameNames.Contains(_gameData.GameName) == false)
        {
            AllGameNames.Add(_gameData.GameName);
        }

        UpdateGamesList();
    }

    public void LoadGame(string gameName)
    {
         string text = PlayerPrefs.GetString(gameName);
         _gameData = JsonUtility.FromJson<GameData>(text);
        SceneManager.LoadScene(1);
    }

    void HandlePlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("HandlePlayerJoined " + playerInput);
        PlayerData playerData = GetPlayerData(playerInput.playerIndex);

        Player player = playerInput.GetComponent<Player>();
        player.Bind(playerData);
    }

    PlayerData GetPlayerData(int playerIndex)
    {
        if (_gameData.PlayerDatas.Count <= playerIndex)
        {
            var playerData = new PlayerData();
            _gameData.PlayerDatas.Add(playerData);
        }

        return _gameData.PlayerDatas[playerIndex];
    }

    public void NewGame()
    {
        Debug.Log("NewGame Called");
        _gameData = new GameData(DateTime.Now.ToString("G"));
        SceneManager.LoadScene(1);
    }

    public void DeleteGame(string gameName)
    {
        PlayerPrefs.DeleteKey(gameName);
        AllGameNames.Remove(gameName);
        UpdateGamesList();
    }

    void UpdateGamesList()
    {
        string commaSeperatedNames = string.Join(",", AllGameNames);
        PlayerPrefs.SetString("AllGameNames", commaSeperatedNames);
        PlayerPrefs.Save();
    }
}