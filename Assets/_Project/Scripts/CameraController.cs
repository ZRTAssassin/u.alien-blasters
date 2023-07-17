using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : NetworkBehaviour
{
    [SerializeField] GameObject _cameraHolder;
    [SerializeField] Vector3 _offset;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _cameraHolder.SetActive(true);
    }

    void Update()
    {
        _cameraHolder.transform.position = transform.position + _offset;
    }
}