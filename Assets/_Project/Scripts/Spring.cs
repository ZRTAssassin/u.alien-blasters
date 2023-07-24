using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts
{
    public class Spring : NetworkBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] List<Sprite> _sprites;
        Sprite _defaultSprite;
        [SerializeField] Sprite _sprungSprite;

        [Header("Audio"), Space(5)] [SerializeField]
        AudioSource _audioSource;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultSprite = _spriteRenderer.sprite;
            _audioSource = GetComponent<AudioSource>();
            _sprites.Add(_defaultSprite);
            _sprites.Add(_sprungSprite);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                //_spriteRenderer.sprite = _sprungSprite;
                UpdateSpriteServerRpc(_sprites.IndexOf(_sprungSprite));
                _audioSource.Play();
            }

        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                //_spriteRenderer.sprite = _defaultSprite;
                UpdateSpriteServerRpc(_sprites.IndexOf(_defaultSprite));
            }
        }
        [ServerRpc(RequireOwnership = false)]
        void UpdateSpriteServerRpc(int spriteIndex)
        {
            UpdateSpriteClientRpc(spriteIndex);
        }
        [ClientRpc]
        void UpdateSpriteClientRpc(int spriteIndex)
        {
            UpdateSprite(spriteIndex);
        }

        void UpdateSprite(int spriteIndex)
        {
            _spriteRenderer.sprite = _sprites[spriteIndex];
        }
    }
}
