using UnityEngine;

namespace _Project.Scripts
{
    public class Frog : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rb;
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] AudioSource _audioSource;

        [Header("Jump Setup"), Space(5)] [SerializeField]
        float _jumpDelay = 3.0f;

        [SerializeField] Vector2 _jumpForce;
        [SerializeField] int _maxJumpsBeforeSwapping = 1;
        [SerializeField] int _jumpsRemaining;

        [Header("Sprite Setup"), Space(5)] Sprite _defaultSprite;
        [SerializeField] Sprite _jumpSprite;


        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultSprite = _spriteRenderer.sprite;
            InvokeRepeating(nameof(Jump), _jumpDelay, _jumpDelay);
            _jumpsRemaining = _maxJumpsBeforeSwapping;
        }

        void Jump()
        {

            if (_jumpsRemaining <= 0)
            {
                _jumpsRemaining = _maxJumpsBeforeSwapping;
                _jumpForce *= new Vector2(-1, 1);
                
            }
            _jumpsRemaining--;
            _rb.AddForce(_jumpForce);
            _spriteRenderer.flipX = _jumpForce.x > 0;
            _spriteRenderer.sprite = _jumpSprite;
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            _spriteRenderer.sprite = _defaultSprite;
            _audioSource.Play();
        }
    }
}