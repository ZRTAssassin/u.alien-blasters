using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class Spikes : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
