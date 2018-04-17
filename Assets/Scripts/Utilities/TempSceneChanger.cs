using UnityEngine.SceneManagement;
using UnityEngine;

public class TempSceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}