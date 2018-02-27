using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FirstScript : MonoBehaviour
{
    private Canvas[] canvas;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        canvas = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].worldCamera = camera;
        }
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyUp(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void ResetGame()
    {
        PlayerPrefs.SetInt("firstFarms", 0);
        PlayerPrefs.SetInt("playerProfile", 0);
        PlayerPrefs.SetInt("playerInventory", 0);
        PlayerPrefs.SetInt("firstGrass", 0);
        PlayerPrefs.SetInt("firstField", 0);
        PlayerPrefs.SetInt("firstBuilding", 0);
        StartCoroutine("RestartGame");
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }
}