using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [Header("Debug Setup"), Space(5)] [SerializeField]
        Rigidbody2D _rb;

        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] Animator _animator;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] PlayerInput _playerInput;

        [Space(5)] [SerializeField] float _footOffset = 0.5f;


        [Header("Input setup"), Space(5)] [SerializeField]
        float _horizontal;
        [SerializeField] float horizontalInput = 0f;

        [Header("Movement setup"), Space(5)] [SerializeField]
        float _maxHorizontalSpeed = 5.0f;


        [SerializeField] float _groundAcceleration = 10;
        [SerializeField] float _snowAcceleration = 1;
        [SerializeField] bool _isOnSnow;

        #region AnimatorStrings

        static readonly int AnimIsGrounded = Animator.StringToHash("IsGrounded");
        static readonly int AnimHorizontalSpeed = Animator.StringToHash("HorizontalSpeed");

        #endregion


        #region Jump Setup

        [Header("Jump setup"), Space(5)] [SerializeField]
        float _jumpEndTime;

        [SerializeField] float _jumpVelocity = 5.0f;
        [SerializeField] float _jumpDuration = 0.25f;
        [SerializeField] float _groundedRayDistance = 0.1f;
        [SerializeField] bool _isGrounded;
        [SerializeField] int _jumpsRemaining;
        [SerializeField] LayerMask _layerMask;
        

        #endregion


        #region Inventory System
        
        [SerializeField] int _coins;

        #endregion
        
        public bool IsGrounded => _isGrounded;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGrounding();

            horizontalInput = _playerInput.actions["Move"].ReadValue<Vector2>().x;
            var vertical = _rb.velocity.y;
            if (_playerInput.actions["Jump"].WasPerformedThisFrame() && _jumpsRemaining > 0)
            {
                _jumpEndTime = Time.time + _jumpDuration;
                _jumpsRemaining--;

                _audioSource.pitch = (_jumpsRemaining) > 0 ? 1 : 0.8f;

                _audioSource.Play();
            }

            if (_playerInput.actions["Jump"].ReadValue<float>() > 0.5f && _jumpEndTime > Time.time)
            {
                vertical = _jumpVelocity;
            }

            var desiredHorizontal = horizontalInput * _maxHorizontalSpeed;
            var acceleration = (_isOnSnow) ? _snowAcceleration : _groundAcceleration;
            _horizontal = Mathf.Lerp(_horizontal, desiredHorizontal, Time.deltaTime * acceleration);
            _rb.velocity = new Vector2(_horizontal, vertical);
            UpdateSprite();
        }

        void UpdateGrounding()
        {
            _isGrounded = false;
            _isOnSnow = false;
            
            //check center
            Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
            var hit = Physics2D.Raycast(origin, Vector2.down, _groundedRayDistance, _layerMask);
            if (hit.collider)
            {
                _isGrounded = true;
                _isOnSnow = hit.collider.CompareTag("Snow");
            }

            //check left
            origin = new Vector2(transform.position.x - _footOffset,
                transform.position.y - _spriteRenderer.bounds.extents.y);
            hit = Physics2D.Raycast(origin, Vector2.down, _groundedRayDistance, _layerMask);
            if (hit.collider)
            {
                _isGrounded = true;
                _isOnSnow = hit.collider.CompareTag("Snow");
            }

            //check right
            origin = new Vector2(transform.position.x + _footOffset,
                transform.position.y - _spriteRenderer.bounds.extents.y);
            hit = Physics2D.Raycast(origin, Vector2.down, _groundedRayDistance, _layerMask);
            if (hit.collider)
            {
                _isGrounded = true;
                _isOnSnow = hit.collider.CompareTag("Snow");
            }

            if (_isGrounded && _rb.velocity.y <= 0)
            {
                _jumpsRemaining = 2;
            }
        }

        void UpdateSprite()
        {
            _animator.SetBool(AnimIsGrounded, _isGrounded);
            _animator.SetFloat(AnimHorizontalSpeed, Math.Abs(_horizontal));
            if (_horizontal > 0)
                _spriteRenderer.flipX = false;
            else if (_horizontal < 0)
                _spriteRenderer.flipX = true;
        }

        void OnDrawGizmos()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            Gizmos.color = Color.red;
            var transformPosition = transform.position;

            Vector2 origin = new Vector2(transformPosition.x, transformPosition.y - spriteRenderer.bounds.extents.y);
            Gizmos.DrawLine(origin, origin + Vector2.down * _groundedRayDistance);

            //Draw Left Foot

            origin = new Vector2(transformPosition.x - _footOffset,
                transformPosition.y - spriteRenderer.bounds.extents.y);
            Gizmos.DrawLine(origin, origin + Vector2.down * _groundedRayDistance);

            // Draw Right Food
            origin = new Vector2(transformPosition.x + _footOffset,
                transformPosition.y - spriteRenderer.bounds.extents.y);
            Gizmos.DrawLine(origin, origin + Vector2.down * _groundedRayDistance);
        }

        public void AddPoint()
        {
            _coins++;
        }
    }
}