using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovingPlatform : NetworkBehaviour
{
    [SerializeField] List<Vector3> _positions = new List<Vector3>();
    [SerializeField] Vector3 _position1;
    [SerializeField] Vector3 _position2;
    [Range(0f, 2f)] [SerializeField] float _platformMoveSpeed = 1f;
    [Range(0f, 1f)] [SerializeField] float _percentAcross;

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        _percentAcross = Mathf.PingPong(NetworkManager.Singleton.LocalTime.TimeAsFloat * _platformMoveSpeed, 1f);
        transform.position = Vector3.Lerp(_position1, _position2, _percentAcross);
        // Move up and down by 5 meters and change direction every 3 seconds.
        // var positionY = Mathf.PingPong(NetworkManager.Singleton.LocalTime.TimeAsFloat / 3f, 1f) * 5f;
        // transform.position = new Vector3(0, positionY, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var collider = GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube(_position1, collider.bounds.size);
        Gizmos.DrawWireCube(_position2, collider.bounds.size);

        Gizmos.color = Color.yellow;
        var currentPosiotn = Vector3.Lerp(_position1, _position2, _percentAcross);
        Gizmos.DrawWireCube(currentPosiotn, collider.bounds.size);
    }

    [ContextMenu("SetPosition1")]
    public void SetPosition1() => _position1 = transform.position;

    [ContextMenu("SetPosition2")]
    public void SetPosition2() => _position1 = transform.position;

    void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            //TODO FIX ME
            Debug.Log("Hit player. Need to fix set parent stuff on hitting.");
            //player.transform.SetParent(transform);
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            //TODO FIX ME
            Debug.Log("Player left. Need to fix parenting stuff.");
            //player.transform.SetParent(null);
        }
    }
}