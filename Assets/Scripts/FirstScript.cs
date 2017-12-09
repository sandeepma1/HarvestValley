using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FirstScript : MonoBehaviour
{
    public static FirstScript Instance = null;

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(2))
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
        StartCoroutine("RestartGame");
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }
}
