using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] PlayerInputManager _playerInputManager;
        [SerializeField] GameData _gameData;
        public static GameManager Instance { get; private set; }

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

            _playerInputManager = GetComponent<PlayerInputManager>();
            _playerInputManager.onPlayerJoined += HandlePlayerJoined;

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex == 0)
            {
                _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            }
            else
            {
                _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            }
        }

        void HandlePlayerJoined(PlayerInput playerInput)
        {
            Debug.Log($"{gameObject.name}: HanldePlayerJoined called (). {playerInput}");
            PlayerData playerData = GetPlayerData(playerInput.playerIndex);

            var player = playerInput.GetComponent<Player.Player>();
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
            Debug.Log("NewGameCalled");
            _gameData = new GameData();
            SceneManager.LoadScene(1);
        }
    }
}