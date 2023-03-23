using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Player
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;
        [SerializeField] Image[] _hearts;
        Player _player;

        public void Bind(Player player)
        {
            _player = player;
            _player.CoinsChanged += UpdateCoinsText;
            UpdateCoinsText();
        }

        void UpdateCoinsText()
        {
            _text.SetText(_player.Coins.ToString());
        }

        void Update()
        {
            if (_player != null)
            {
                for (int i = 0; i < _hearts.Length; i++)
                {
                    Image heart = _hearts[i];
                    heart.enabled = i < _player.Health;
                }
            }
        }
    }
}