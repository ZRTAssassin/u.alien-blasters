using UnityEngine;

namespace _Project.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rb;
        [SerializeField] SpriteRenderer _spriteRenderer;

        [Header("Jump setup"), Space(5)]
        [SerializeField] float _jumpEndTime;
        [SerializeField] float _horizontalVelocity = 3.0f;
        [SerializeField] float _jumpVelocity = 5.0f;
        [SerializeField] float _jumpDuration = 0.25f;
        [SerializeField] float _groundedRayDistance = 0.1f;
        [SerializeField] bool _isGrounded;

        public bool IsGrounded => _isGrounded;

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
            
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = _rb.velocity.y;
            if (Input.GetButtonDown("Fire1") && _isGrounded)
            {
                _jumpEndTime = Time.time + _jumpDuration;
            }

            if (Input.GetButton("Fire1") && _jumpEndTime > Time.time) 
            {
                vertical = _jumpVelocity;
            }

            horizontal *= _horizontalVelocity; 
            _rb.velocity = new Vector2(horizontal, vertical);
        }

        void OnDrawGizmos()
        {
        
            Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + Vector2.down * _groundedRayDistance);
        }
    }
}
