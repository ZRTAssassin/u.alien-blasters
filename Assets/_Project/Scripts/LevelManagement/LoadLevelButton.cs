using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.LevelManagement
{
    public class LoadLevelButton : MonoBehaviour
    {
        public void LoadLevel(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }
    }
}