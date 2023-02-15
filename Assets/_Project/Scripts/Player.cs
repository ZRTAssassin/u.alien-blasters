using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rb;
        [SerializeField] SpriteRenderer _spriteRenderer;
        

        [Header("Input setup"), Space(5)]
        [SerializeField] float _horizontal;


        [Header("Jump setup"), Space(5)] [SerializeField]
        float _jumpEndTime;

        [SerializeField] float _horizontalVelocity = 3.0f;
        [SerializeField] float _jumpVelocity = 5.0f;
        [SerializeField] float _jumpDuration = 0.25f;
        [SerializeField] float _groundedRayDistance = 0.1f;
        [SerializeField] bool _isGrounded;

        [Header("Sprite Setup"), Space(5)] [SerializeField]
        Sprite _jumpSprite;

        [SerializeField] Sprite _defaultSprite;


        public bool IsGrounded => _isGrounded;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultSprite = _spriteRenderer.sprite;
        }

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
            var hit = Physics2D.Raycast(origin, Vector2.down, _groundedRayDistance);
            if (hit.collider)
            {
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }

            _horizontal = Input.GetAxis("Horizontal");
            var vertical = _rb.velocity.y;
            if (Input.GetButtonDown("Fire1") && _isGrounded)
            {
                _jumpEndTime = Time.time + _jumpDuration;
            }

            if (Input.GetButton("Fire1") && _jumpEndTime > Time.time)
            {
                vertical = _jumpVelocity;
            }

            _horizontal *= _horizontalVelocity;
            _rb.velocity = new Vector2(_horizontal, vertical);
            UpdateSprite();
        }

        void UpdateSprite()
        {
            _spriteRenderer.sprite = _isGrounded ? _defaultSprite : _jumpSprite;
            if (_horizontal > 0)
                _spriteRenderer.flipX = false;
            else if (_horizontal < 0)
                _spriteRenderer.flipX = true;

        }

        void OnDrawGizmos()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + Vector2.down * _groundedRayDistance);
        }
    }
}