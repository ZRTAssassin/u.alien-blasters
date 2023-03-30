using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _left;
    [SerializeField] Sprite _right;

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
        _spriteRenderer.sprite = _left;
    }

    void TurnOn()
    {
        _spriteRenderer.sprite = _right;
    }
}