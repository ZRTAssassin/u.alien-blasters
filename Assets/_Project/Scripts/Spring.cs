using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class Spring : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        Sprite _defaultSprite;
        [SerializeField] Sprite _sprungSprite;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultSprite = _spriteRenderer.sprite;
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                _spriteRenderer.sprite = _sprungSprite;
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
