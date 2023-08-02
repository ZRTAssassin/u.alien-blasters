using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.LevelManagement
{
    public class LoadGameButton : MonoBehaviour
    {
        [SerializeField] string _gameName;
        void Start() => GetComponent<Button>().onClick.AddListener(LoadGame);

        public void LoadGame() => GameManager_Old.Instance.LoadGame(_gameName);

        public void DeleteGame()
        {
            GameManager_Old.Instance.DeleteGame(_gameName);
            Destroy(gameObject);
        }

        public void SetGameName(string gameName)
        {
            _gameName = gameName;
            GetComponentInChildren<TMP_Text>().SetText(_gameName);
        }
    }
}