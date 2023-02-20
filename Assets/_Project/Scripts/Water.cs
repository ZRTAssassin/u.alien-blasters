using UnityEngine;

namespace _Project.Scripts
{
    public class Water : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
        }
    }
}
