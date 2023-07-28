using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Laser : NetworkBehaviour
{
    LineRenderer _lineRenderer;
    [SerializeField] bool _isOn;
    [SerializeField] Vector2 _direction = Vector2.left;
    [SerializeField] float _distance = 10f;
    [SerializeField] SpriteRenderer _laserBurst;

    NetworkVariable<bool> _laserEnabled = new NetworkVariable<bool>();
    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Toggle(false);
        //ToggleServerRpc(false);
    }

    void Update()
    {
        if (!_isOn)
        {
            //_laserBurst.enabled = false;
            SetBurstServerRpc(false);
            return;
        }

        var endpoint = (Vector2)transform.position + (_direction * _distance);

        var firstThing = Physics2D.Raycast(transform.position, _direction, _distance);
        if (firstThing.collider)
        {
            endpoint = firstThing.point;
            //_laserBurst.enabled = true;
            SetBurstServerRpc(true);
            _laserBurst.transform.position = endpoint;
            _laserBurst.transform.localScale = Vector3.one * (0.7f + Mathf.PingPong(Time.time, 1f));
            
            var laserDamagable = firstThing.collider.GetComponent<ITakeLaserDamage>();
            if (laserDamagable != null)
            {
                laserDamagable.TakeLaserDamage();
            }
        }
        _lineRenderer.SetPosition(1, endpoint);
    }


    [ServerRpc(RequireOwnership = false)]
    public void ToggleServerRpc(bool value)
    {
        ToggletClientRpc(value);
    }
    [ClientRpc]
    void ToggletClientRpc(bool value)
    {
        Toggle(value);
    }
    private void Toggle(bool state)
    {
        _isOn = state;
        _lineRenderer.enabled = state;
    }

    [ServerRpc(RequireOwnership = false)]
    void SetBurstServerRpc(bool value)
    {
        SetBurstClientRpc(value);
    }
[ClientRpc]
    void SetBurstClientRpc(bool value)
    {
        SetBurst(value);
    }

    void SetBurst(bool value)
    {
        _laserBurst.enabled = value;
    }
}
