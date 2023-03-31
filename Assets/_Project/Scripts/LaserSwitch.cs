using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _left;
    [SerializeField] Sprite _right;

    [SerializeField] UnityEvent _on;
    [SerializeField] UnityEvent _off;
    [SerializeField] bool _isOn;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
            _spriteRenderer.sprite = _left;
            _off?.Invoke();
        }
    }

    void TurnOn()
    {
        if (!_isOn)
        {
            _isOn = true;
            _spriteRenderer.sprite = _right;
            _on?.Invoke();
            
        }
    }
}