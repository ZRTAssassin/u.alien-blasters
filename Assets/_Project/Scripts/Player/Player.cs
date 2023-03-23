using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        #region DebugSetup

        

        [Header("Debug Setup"), Space(5)] [SerializeField]
        Rigidbody2D _rb;

        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] Animator _animator;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] PlayerInput _playerInput;

        [Space(5)] [SerializeField] float _footOffset = 0.5f;
        #endregion

        #region Input Setup

        

        [Header("Input setup"), Space(5)] [SerializeField]
        float _horizontal;

        [SerializeField] float horizontalInput = 0f;
        #endregion

        #region Movement Setup

        

        [Header("Movement setup"), Space(5)] [SerializeField]
        float _maxHorizontalSpeed = 5.0f;


        [SerializeField] float _groundAcceleration = 10;
        [SerializeField] float _snowAcceleration = 1;
        [SerializeField] bool _isOnSnow;

        #endregion
        
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

        #region Damage Setup

        [Header("Damage setup"), Space(5)] [SerializeField]
        float _knockback = 300;
        [SerializeField] List<AudioClip> _hurtSounds = new List<AudioClip>();

        #endregion

        #region Coin System

        [Header("Coin setup"), Space(5)] [SerializeField]
        List<AudioClip> _coinSounds = new List<AudioClip>();

        public int Coins
        {
            get => _playerData.Coins;
            private set => _playerData.Coins = value;
        }

        #endregion


        public bool IsGrounded => _isGrounded;


        #region Data Region

        PlayerData _playerData = new PlayerData();
        public int Health => _playerData.Health;
        #endregion

        public event Action CoinsChanged;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();

            FindObjectOfType<PlayerCanvas>().Bind(this);
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
            Coins++;
            CoinsChanged?.Invoke();
            var number = Random.Range(0, _coinSounds.Count - 1);
            var soundClip = _coinSounds[number];
            if (soundClip != null)
                _audioSource.PlayOneShot(soundClip);
        }

        public void Bind(PlayerData playerData)
        {
            _playerData = playerData;
            
        }

        public void TakeDamage(Vector2 hitNormal)
        {
            _playerData.Health--;
            if (_playerData.Health <= 0)
            {
                SceneManager.LoadScene(0);
                return;
            }

            _rb.AddForce(-hitNormal * _knockback);

            var number = Random.Range(0, _hurtSounds.Count - 1);
            var soundClip = _hurtSounds[number];
            if (soundClip != null)
            {
                _audioSource.PlayOneShot(soundClip);
            }
        }
    }
}