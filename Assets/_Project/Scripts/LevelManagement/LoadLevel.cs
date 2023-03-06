using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.LevelManagement
{
    public class LoadLevel : MonoBehaviour
    {
        [SerializeField] string _levelName;

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                SceneManager.LoadScene(_levelName);
            }
        }
    }
}
