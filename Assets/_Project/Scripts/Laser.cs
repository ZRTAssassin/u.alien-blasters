using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer _lineRenderer;
    [SerializeField] bool _isOn;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Toggle(false);
    }

    public void Toggle(bool state)
    {
        _isOn = state;
        _lineRenderer.enabled = state;
    }
}
