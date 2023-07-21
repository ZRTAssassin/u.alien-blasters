using Unity.Netcode;
using UnityEngine;

public class Ladybug : NetworkBehaviour, ITakeLaserDamage
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D _rigidbody;

    [SerializeField] Vector2 _direction = Vector2.left;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _raycastDistance = 0.2f;
    [SerializeField] LayerMask _forwardRaycastLayerMask;

    NetworkVariable<bool> _isFacingRight = new();

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _isFacingRight.OnValueChanged += HandleIsFacingLeftChanged;
    }

    void HandleIsFacingLeftChanged(bool previousvalue, bool newvalue)
    {
        _spriteRenderer.flipX = newvalue;
    }


    void OnDrawGizmos()
    {
        var collider = GetComponent<Collider2D>();
        Gizmos.color = Color.red;

        Vector2 offset = _direction * collider.bounds.extents.x;
        Vector2 origin = (Vector2)transform.position + offset;
        Gizmos.DrawLine(origin, origin + (_direction * _raycastDistance));

        var downOrigin = GetDownRayPosition(collider);
        Gizmos.DrawLine(downOrigin, downOrigin + (Vector2.down * _raycastDistance));
    }

    Vector2 GetDownRayPosition(Collider2D collider)
    {
        var bounds = collider.bounds;
        if (_direction == Vector2.left)
        {
            return new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y);
        }
        else
        {
            return new Vector2(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y);
        }
    }

    void Update()
    {
        if (!IsServer) return;
        CheckGroundInFront();
        CheckIntFront();


        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
    }

    void CheckIntFront()
    {
        Vector2 offset = _direction * _collider.bounds.extents.x;
        Vector2 origin = (Vector2)transform.position + offset;
        var hits = Physics2D.RaycastAll(origin, _direction, _raycastDistance, _forwardRaycastLayerMask);
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                _direction *= -1;
                //_spriteRenderer.flipX = _direction == Vector2.right;
                _isFacingRight.Value = _direction == Vector2.right;
                break;
            }
        }
    }

    void CheckGroundInFront()
    {
        bool canContinueWalking = false;
        var downOrigin = GetDownRayPosition(_collider);
        var dowHits = Physics2D.RaycastAll(downOrigin, Vector2.down, _raycastDistance);

        foreach (var downHit in dowHits)
        {
            if (downHit.collider != null && downHit.collider.gameObject != gameObject)
            {
                canContinueWalking = true;
            }
        }

        if (!canContinueWalking)
        {
            _direction *= -1;
            // _spriteRenderer.flipX = _direction == Vector2.right;
            _isFacingRight.Value = _direction == Vector2.right;
            return;
        }
    }

    public void TakeLaserDamage()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}