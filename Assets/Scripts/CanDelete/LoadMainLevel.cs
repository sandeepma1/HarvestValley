using UnityEngine;

public class LoadMainLevel : Singleton<LoadMainLevel>
{
    public GameObject loadingScreen;

    public void LoadMainScene_SandBox()
    {
        loadingScreen.SetActive(true);
        MySceneManager.LoadScene("Main_SB");
    }

    public void LoadMainScene_ProceduralGeneration()
    {
        loadingScreen.SetActive(true);
        MySceneManager.LoadScene("Main_PG");
    }

    public void LoadMainScene_ProceduralGeneration_Portrait()
    {
        loadingScreen.SetActive(true);
        MySceneManager.LoadScene("Main_PG_Portrait");
    }

    public void LoadMainScene_SpriteLightDemo()
    {
        loadingScreen.SetActive(true);
        MySceneManager.LoadScene("SpriteLightKitScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}