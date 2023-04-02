using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer _lineRenderer;
    [SerializeField] bool _isOn;
    [SerializeField] Vector2 _direction = Vector2.left;
    [SerializeField] float _distance = 10f;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Toggle(false);
    }

    void Update()
    {
        if (!_isOn)
            return;
        var endpoint = (Vector2)transform.position + (_direction * _distance);

        var firstThing = Physics2D.Raycast(transform.position, _direction, _distance);
        if (firstThing.collider)
        {
            endpoint = firstThing.point;
            var laserDamagable = firstThing.collider.GetComponent<ITakeLaserDamage>();
            if (laserDamagable != null)
            {
                laserDamagable.TakeLaserDamage();
            }
        }
        _lineRenderer.SetPosition(1, endpoint);
    }

    public void Toggle(bool state)
    {
        _isOn = state;
        _lineRenderer.enabled = state;
    }
}
