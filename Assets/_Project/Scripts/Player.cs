using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;

    [Header("Jump setup"), Space(5)]
    [SerializeField] float _jumpEndTime;
    [SerializeField] float _jumpVelocity = 5.0f;
    [SerializeField] float _jumpDuration = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = _rb.velocity.y;
        if (Input.GetButtonDown("Fire1"))
        {
            _jumpEndTime = Time.time + _jumpDuration;
        }

        if (Input.GetButton("Fire1") && _jumpEndTime > Time.time) 
        {
            vertical = _jumpVelocity;
        }
        _rb.velocity = new Vector2(horizontal, vertical);
    }
}
