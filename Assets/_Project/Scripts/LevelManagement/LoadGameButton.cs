using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.LevelManagement
{
    public class LoadGameButton : MonoBehaviour
    {
        void Start() => GetComponent<Button>().onClick.AddListener(LoadGame);

        public void LoadGame() => GameManager.Instance.LoadGame();
    }
}