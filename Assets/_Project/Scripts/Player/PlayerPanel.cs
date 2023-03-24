using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Player
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;
        [SerializeField] Image[] _hearts;
        [SerializeField] Image _flashImage;
        Player _player;

        public void Bind(Player player)
        {
            _player = player;
            _player.CoinsChanged += UpdateCoinsText;
            _player.HealthChanged += UpdateHealth;
            UpdateCoinsText();
            UpdateHealth();
        }

        void UpdateCoinsText()
        {
            _text.SetText(_player.Coins.ToString());
        }

        void UpdateHealth()
        {
            for (int i = 0; i < _hearts.Length; i++)
            {
                Image heart = _hearts[i];
                heart.enabled = i < _player.Health;
            }

            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            _flashImage.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _flashImage.enabled = false;
        }
    }
}