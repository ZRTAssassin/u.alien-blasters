using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] PlayerInputManager _playerInput;
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

            _playerInput = GetComponent<PlayerInputManager>();
            _playerInput.onPlayerJoined += HandlePlayerJoined;

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
            if (_playerDatas.Count <= playerIndex)
            {
                var playerData = new PlayerData();
                _playerDatas.Add(playerData);
            }

            return _playerDatas[playerIndex];
        }

        List<PlayerData> _playerDatas = new List<PlayerData>();
    }
}
