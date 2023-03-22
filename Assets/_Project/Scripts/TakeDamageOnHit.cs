using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class TakeDamageOnHit : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                var player = other.collider.GetComponent<Player.Player>();
                if (player)
                {
                    player.TakeDamage();
                }
            }
        }
    }
}
