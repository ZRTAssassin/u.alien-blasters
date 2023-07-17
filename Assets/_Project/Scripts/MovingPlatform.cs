using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Vector3> _positions = new List<Vector3>();
    [SerializeField] Vector3 _position1;
    [SerializeField] Vector3 _position2;
    [Range(0f, 2f)] [SerializeField] float _platformMoveSpeed = 1f;
    [Range(0f, 1f)] [SerializeField] float _percentAcross;

    // Update is called once per frame
    void Update()
    {
        _percentAcross = Mathf.PingPong(Time.time * _platformMoveSpeed, 1f);
        transform.position = Vector3.Lerp(_position1, _position2, _percentAcross);
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
            player.transform.SetParent(null);
        }
    }
}