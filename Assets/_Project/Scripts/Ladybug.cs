using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladybug : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D _rigidbody;

    [SerializeField] Vector2 _direction;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _raycastDistance = 0.2f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _direction = Vector2.left;
    }

    void Update()
    {
        Vector2 offset = _direction * _collider.bounds.extents.x;
        Vector2 origin = (Vector2)transform.position + offset;

        var hits = Physics2D.RaycastAll(origin, _direction, _raycastDistance);
        Debug.DrawRay(origin, _direction, Color.red);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                _direction *= -1;
                _spriteRenderer.flipX = _direction == Vector2.right;
                break;
            }
        }
        
        
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
    }
    
    
}
