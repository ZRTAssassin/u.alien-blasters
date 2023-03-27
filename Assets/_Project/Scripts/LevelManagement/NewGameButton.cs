using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.LevelManagement
{
    public class NewGameButton : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(CreateNewGame);
        }

        public void CreateNewGame()
        {
            GameManager.Instance.NewGame();
        }
    }
}
