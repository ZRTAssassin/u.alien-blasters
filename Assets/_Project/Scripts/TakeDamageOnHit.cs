using UnityEngine;
using UnityEngine.SceneManagement;


    public class TakeDamageOnHit : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D other)
        {
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

