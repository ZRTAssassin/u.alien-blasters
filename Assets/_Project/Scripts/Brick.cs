using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour, ITakeLaserDamage
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] ParticleSystem _particles;
    [SerializeField] float _takenDamageTime;
    [SerializeField] float _laserDestructionTime = 1f;
    [SerializeField] float _resetColorTime;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if ( _resetColorTime > 0 && Time.time >= _resetColorTime)
        {
            _resetColorTime = 0;
            _spriteRenderer.color = Color.white;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player == null)
        {
            return;
        }

        Vector2 normal = other.contacts[0].normal;
        float dot = Vector2.Dot(normal, Vector2.up);
        Debug.Log(dot);
        if (dot > 0.5)
        {
            player.StopJump();
            Explode();

        }
    }

    void Explode()
    {
        Instantiate(_particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void TakeLaserDamage()
    {
        _spriteRenderer.color = Color.red;
        _resetColorTime = Time.time + 0.1f;
        _takenDamageTime += Time.deltaTime;
        if ( _takenDamageTime >= _laserDestructionTime)
        {
            Explode();
        }
    }
}
