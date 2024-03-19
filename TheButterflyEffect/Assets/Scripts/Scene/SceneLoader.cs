using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadSceneInstance(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneInstance(int index)
    {
        SceneManager.LoadScene(index);
    }
}