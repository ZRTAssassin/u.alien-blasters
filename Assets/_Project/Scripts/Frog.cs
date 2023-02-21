using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] float _jumpDelay = 3.0f;
    [SerializeField] Vector2 _jumpForce;
    
    [Header("Sprite Setup"), Space(5)]
    Sprite _defaultSprite;
    [SerializeField] Sprite _jumpSprite;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
        InvokeRepeating(nameof(Jump), _jumpDelay, _jumpDelay);
    }

    void Jump()
    {
        _rb.AddForce(_jumpForce);
        _jumpForce *= new Vector2(-1, 1);
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        _spriteRenderer.sprite = _jumpSprite;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        _spriteRenderer.sprite = _defaultSprite;
    }
}
