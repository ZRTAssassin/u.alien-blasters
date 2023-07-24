using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class LaserSwitch : NetworkBehaviour
{
    [SerializeField] List<Sprite> _sprites = new();
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _left;
    [SerializeField] Sprite _right;

    [SerializeField] UnityEvent _on;
    [SerializeField] UnityEvent _off;
    [SerializeField] bool _isOn;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sprites.Add(_left);
        _sprites.Add(_right);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player is null)
        {
            return;
        }

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb.velocity.x > 0)
        {
            TurnOn();
        }
        else if (rb.velocity.x < 0)
        {
            TurnOff();
        }
    }

    void TurnOff()
    {
        if (_isOn)
        {
            _isOn = false;
            //_spriteRenderer.sprite = _left;
            // send new sprite ID over the RPC
            UpdateSpriteServerRpc(_sprites.IndexOf(_left));
            _off?.Invoke();
        }
    }

    void TurnOn()
    {
        if (!_isOn)
        {
            _isOn = true;
            //_spriteRenderer.sprite = _right;
            UpdateSpriteServerRpc(_sprites.IndexOf(_right));
            _on?.Invoke();
            
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