using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] ParticleSystem _particles;
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
            Instantiate(_particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
