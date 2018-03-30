using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelByName : MonoBehaviour
{
    public string levelName = "Menu";

    void Start()
    {
        SceneManager.LoadScene(levelName);
    }
}
