using UnityEngine;

public class ListenerSingleton : MonoBehaviour
{
    private static ListenerSingleton instance;

    public static ListenerSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("AudioListenerSingleton").AddComponent<ListenerSingleton>();
                instance.gameObject.AddComponent<AudioListener>();
                DontDestroyOnLoad(instance);
            }

            return instance;
        }

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<AudioListener>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}