using UnityEngine;

public class LoadMainLevel : Singleton<LoadMainLevel>
{
    public GameObject loadingScreen;

    public void LoadMainScene_SandBox()
    {
        loadingScreen.SetActive(true);
        SceneChanger.Instance.LoadScene("Main_SB");
    }

    public void LoadMainScene_ProceduralGeneration()
    {
        loadingScreen.SetActive(true);
        SceneChanger.Instance.LoadScene("Main_PG");
    }

    public void LoadMainScene_ProceduralGeneration_Portrait()
    {
        loadingScreen.SetActive(true);
        SceneChanger.Instance.LoadScene("Main_PG_Portrait");
    }

    public void LoadMainScene_SpriteLightDemo()
    {
        loadingScreen.SetActive(true);
        SceneChanger.Instance.LoadScene("SpriteLightKitScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}