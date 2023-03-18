using TMPro;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;
        Player _player;

        public void Bind(Player player)
        {
            _player = player;
        }

        void Update()
        {
            if (_player != null)
                _text.SetText(_player.Coins.ToString());
        }
    }
}
