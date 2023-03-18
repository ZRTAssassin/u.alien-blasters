using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
    public class PlayerCanvas : MonoBehaviour
    {
        [SerializeField] PlayerPanel[] _playerPanels;

        public void Bind(Player player)
        {
            var playerInput = player.GetComponent<PlayerInput>();
            _playerPanels[playerInput.playerIndex].Bind(player);
        }
    }
}
