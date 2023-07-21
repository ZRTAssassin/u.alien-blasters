using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Brick : NetworkBehaviour, ITakeLaserDamage
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] ParticleSystem _particles;
    [SerializeField] float _takenDamageTime;
    [SerializeField] float _laserDestructionTime = 1f;
    [SerializeField] float _resetColorTime;
    [SerializeField] bool _isInvincible = false;

    

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
            UpdateSpriteRenderer(Color.white);
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
        if (_isInvincible) return;
        ExplodeServerRpc();

    }
[ServerRpc(RequireOwnership = false)]
    void ExplodeServerRpc()
    {
        ExplodeClientRpc();
    }
[ClientRpc]
    void ExplodeClientRpc()
    {
        Instantiate(_particles, transform.position, Quaternion.identity);
        if (gameObject.TryGetComponent(out NetworkObject networkObject))
        {
            if (IsServer)
            {
                networkObject.Despawn();
            }
        }
        Destroy(gameObject);
    }

    public void TakeLaserDamage()
    {
        UpdateSpriteRenderer(Color.red);
        _spriteRenderer.color = Color.red;
        _resetColorTime = Time.time + 0.1f;
        _takenDamageTime += Time.deltaTime;
        if ( _takenDamageTime >= _laserDestructionTime)
        {
            Explode();
        }
    }

    void UpdateSpriteRenderer(Color color)
    {
        UpdateSpriteColorServerRpc(ColorHelpers.GetColorPropsFromColor(color));
    }
    [ServerRpc(RequireOwnership = false)]
    void UpdateSpriteColorServerRpc(ColorProps colorProps)
    {
        UpdateSpriteColorClientRpc(colorProps);
    }
    [ClientRpc]
    void UpdateSpriteColorClientRpc(ColorProps colorProps)
    {
        Color newColor = ColorHelpers.GetColorFromColorProps(colorProps);
        _spriteRenderer.color = newColor;
        //_spriteRenderer.color = new Color(colorProps.r, colorProps.g, colorProps.b, colorProps.a);
    }
}
