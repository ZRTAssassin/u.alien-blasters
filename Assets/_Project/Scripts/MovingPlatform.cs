using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Vector3> _positions = new List<Vector3>();
    [SerializeField] Vector3 _position1;
    [SerializeField] Vector3 _position2;
    [Range(0f, 1f)] [SerializeField] float _percentAcross;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(_position1, _position2, _percentAcross);
    }
}