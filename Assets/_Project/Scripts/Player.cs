using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts
{
    public class Player : MonoBehaviour
    {
        [Header("Debug Setup"), Space(5)] [SerializeField]
        Rigidbody2D _rb;

        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] Animator _animator;
        [SerializeField] AudioSource _audioSource;

        [Space(5)] [SerializeField] float _footOffset = 0.5f;


        [Header("Input setup"), Space(5)] [SerializeField]
        float _horizontal;

        #region AnimatorStrings

        static readonly int AnimIsGrounded = Animator.StringToHash("IsGrounded");
        static readonly int AnimHorizontalSpeed = Animator.StringToHash("HorizontalSpeed");

        #endregion
        

        #region Jump Setup
        [Header("Jump setup"), Space(5)] [SerializeField]
        float _jumpEndTime;

        [SerializeField] float _horizontalVelocity = 3.0f;
        [SerializeField] float _jumpVelocity = 5.0f;
        [SerializeField] float _jumpDuration = 0.25f;
        [SerializeField] float _groundedRayDistance = 0.1f;
        [SerializeField] bool _isGrounded;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] int _jumpsRemaining;


        #endregion
        

        public bool IsGrounded => _isGrounded;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGrounding();

            _horizontal = Input.GetAxis("Horizontal");
            var vertical = _rb.velocity.y;
            if (Input.GetButtonDown("Fire1") && _jumpsRemaining > 0)
            {
                _jumpEndTime = Time.time + _jumpDuration;
                _jumpsRemaining--;

                _audioSource.pitch = (_jumpsRemaining) > 0 ? 1 : 0.8f;
                
                _audioSource.Play();
            }

            if (Input.GetButton("Fire1") && _jumpEndTime > Time.time)
            {
                vertical = _jumpVelocity;
            }

            _horizontal *= _horizontalVelocity;
            _rb.velocity = new Vector2(_horizontal, vertical);

            UpdateSprite();
        }

        void UpdateGrounding()
        {
            _isGrounded = false;

            //check center
            Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
            var hit = Physics2D.Raycast(origin, Vector2.down, _groundedRayDistance, _layerMask);
            if (hit.collider)
                _isGrounded = true;
            //check left
            origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
            hit = Physics2D.Raycast(origin, Vector2.down, _groundedRayDistance, _layerMask);
            if (hit.collider)
                _isGrounded = true;
            
            //check right
            origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
            hit = Physics2D.Raycast(origin, Vector2.down, _groundedRayDistance, _layerMask);
            if (hit.collider)
                _isGrounded = true;
            if (_isGrounded && _rb.velocity.y <= 0)
            {
                _jumpsRemaining = 2;
            }
        }

        void UpdateSprite()
        {
            _animator.SetBool(AnimIsGrounded, _isGrounded);
            _animator.SetFloat(AnimHorizontalSpeed, Mathf.Abs(_horizontal));
            if (_horizontal > 0)
                _spriteRenderer.flipX = false;
            else if (_horizontal < 0)
                _spriteRenderer.flipX = true;
        }

        void OnDrawGizmos()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            Gizmos.color = Color.red;

            Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
            Gizmos.DrawLine(origin, origin + Vector2.down * _groundedRayDistance);

            //Draw Left Foot

            origin = new Vector2(transform.position.x - _footOffset,
                transform.position.y - spriteRenderer.bounds.extents.y);
            Gizmos.DrawLine(origin, origin + Vector2.down * _groundedRayDistance);

            // Draw Right Food
            origin = new Vector2(transform.position.x + _footOffset,
                transform.position.y - spriteRenderer.bounds.extents.y);
            Gizmos.DrawLine(origin, origin + Vector2.down * _groundedRayDistance);
        }
    }
}