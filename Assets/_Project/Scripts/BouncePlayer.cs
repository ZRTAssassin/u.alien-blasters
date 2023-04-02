using UnityEngine;

public class BouncePlayer : MonoBehaviour
{
    [SerializeField] bool _onlyFromTop;
    [SerializeField] float _bounciness = 200f;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_onlyFromTop && Vector2.Dot(other.contacts[0].normal, Vector2.down) < 0.5f)
        {
            return;
        }

        if (other.collider.CompareTag("Player"))
        {
            var player = other.collider.GetComponent<Player>();
            if (player)
            {
                player.Bounce(other.contacts[0].normal, _bounciness);
            }
        }
    }
}