using UnityEngine.SceneManagement;

public static class MySceneManager
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}