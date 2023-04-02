using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladybug : MonoBehaviour, ITakeLaserDamage
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D _rigidbody;

    [SerializeField] Vector2 _direction = Vector2.left;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _raycastDistance = 0.2f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnDrawGizmos()
    {
        var collider = GetComponent<Collider2D>();
        Gizmos.color = Color.red;

        Vector2 offset = _direction * collider.bounds.extents.x;
        Vector2 origin = (Vector2)transform.position + offset;
        Gizmos.DrawLine(origin, origin + (_direction * _raycastDistance));

        var downOrigin = GetDownRayPosition(collider);
        Gizmos.DrawLine(downOrigin, downOrigin + (Vector2.down * _raycastDistance));
    }

    Vector2 GetDownRayPosition(Collider2D collider)
    {
        var bounds = collider.bounds;
        if (_direction == Vector2.left)
        {
            return new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y);
        }
        else
        {
            return new Vector2(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y);
        }
    }

    void Update()
    {
        Vector2 offset = _direction * _collider.bounds.extents.x;
        Vector2 origin = (Vector2)transform.position + offset;


        bool canContinueWalking = false;
        var downOrigin = GetDownRayPosition(_collider);
        var dowHits = Physics2D.RaycastAll(downOrigin, Vector2.down, _raycastDistance);

        foreach (var downHit in dowHits)
        {
            if (downHit.collider != null && downHit.collider.gameObject != gameObject)
            {
                canContinueWalking = true;
            }
        }
        
        if (!canContinueWalking)
        {
            _direction *= -1;
            _spriteRenderer.flipX = _direction == Vector2.right;
            return;
        }

        var hits = Physics2D.RaycastAll(origin, _direction, _raycastDistance);


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

    public void TakeLaserDamage()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}