using UnityEngine;
using UnityEngine.SceneManagement;


public class DamagePlayer : MonoBehaviour
{
    [SerializeField] bool _ignoreFromTop;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_ignoreFromTop && Vector2.Dot(other.contacts[0].normal, Vector2.down) > 0.5f)
        {
            return;
        }

        if (other.collider.CompareTag("Player"))
        {
            var player = other.collider.GetComponent<Player>();
            if (player)
            {
                player.TakeDamage(other.contacts[0].normal);
            }
        }
    }
}