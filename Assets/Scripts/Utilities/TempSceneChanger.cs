using UnityEngine;

public class TempSceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneChanger.Instance.LoadScene(sceneName);
    }
}