using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class Spring : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        Sprite _defaultSprite;
        [SerializeField] Sprite _sprungSprite;

        [Header("Audio"), Space(5)] [SerializeField]
        AudioSource _audioSource;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultSprite = _spriteRenderer.sprite;
            _audioSource = GetComponent<AudioSource>();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                _spriteRenderer.sprite = _sprungSprite;
                _audioSource.Play();
            }

        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                _spriteRenderer.sprite = _defaultSprite;
            }
        }
    }
}
