using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : Singleton<SceneChanger>
{
    static Image circleImage;
    static Vector3 maxSize = new Vector3(30, 30, 30);

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        circleImage = transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            FadeIn();
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            FadeOut();
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        FadeIn();
        yield return new WaitForSeconds(1);
        FadeOut();
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Shape will be 0 to max
    /// </summary>
    public static void FadeIn()
    {
        circleImage.transform.localScale = Vector3.zero;
        circleImage.transform.DOScale(maxSize, 1);
    }

    /// <summary>
    /// Shape will be max to 0
    /// </summary>
    public static void FadeOut()
    {
        circleImage.transform.localScale = maxSize;
        circleImage.transform.DOScale(0, 1);
    }
}