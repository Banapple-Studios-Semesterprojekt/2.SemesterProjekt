using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance()
    {
        T tempInstance = FindAnyObjectByType<T>();

        if (instance == null)
        {
            instance = tempInstance;
        }
        else if (instance != tempInstance)
        {
            Destroy(tempInstance);
        }

        //DontDestroyOnLoad(tempInstance);
        return instance;
    }
}