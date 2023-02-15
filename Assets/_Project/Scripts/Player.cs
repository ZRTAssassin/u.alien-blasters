using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;

    [SerializeField] float _jumpEndTime;
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
            _jumpEndTime = Time.time + 0.5f;
        }

        if (Input.GetButton("Fire1") && _jumpEndTime > Time.time) 
        {
            vertical = 5.0f;
        }
        _rb.velocity = new Vector2(horizontal, vertical);
    }
}
