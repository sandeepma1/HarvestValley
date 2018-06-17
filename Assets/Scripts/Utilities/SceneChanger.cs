using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : Singleton<SceneChanger>
{
    static Image circleImage;
    static Vector3 maxSize = new Vector3(30, 30, 30);
    static Material irisMaterial;
    static Vector2 irisShrinkTilling = new Vector2(300, 300);
    static Vector2 irisShrinkOffset = new Vector2(-149.5f, -149.5f);
    static Vector2 irisExpandTilling = new Vector2(0.35f, 0.35f);
    static Vector2 irisExpandOffset = new Vector2(0.35f, 0.35f);
    static float effectSpeed = 1;
    static Ease shrinkEase = Ease.InExpo;
    static Ease expandEase = Ease.OutExpo;
    private Canvas thisCanvas;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        circleImage = transform.GetChild(0).GetComponent<Image>();
        irisMaterial = circleImage.material;
        irisMaterial.mainTextureScale = irisExpandTilling;
        irisMaterial.mainTextureOffset = irisExpandTilling;
        thisCanvas = GetComponent<Canvas>();
        thisCanvas.worldCamera = Camera.main;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.A))
    //    {
    //        FadeIn();
    //    }
    //    if (Input.GetKeyUp(KeyCode.B))
    //    {
    //        FadeOut();
    //    }
    //}

    private void OnLevelWasLoaded(int level)
    {
        StartCoroutine("WaitForEndOfFrameForCamera");
    }

    IEnumerator WaitForEndOfFrameForCamera()
    {
        yield return new WaitForEndOfFrame();
        thisCanvas.worldCamera = Camera.main;
        FadeOut();
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
        irisMaterial.mainTextureScale = irisExpandTilling;
        irisMaterial.mainTextureOffset = irisExpandTilling;
        irisMaterial.DOTiling(irisShrinkTilling, effectSpeed).SetEase(shrinkEase);
        irisMaterial.DOOffset(irisShrinkOffset, effectSpeed).SetEase(shrinkEase);
    }

    /// <summary>
    /// Shape will be max to 0
    /// </summary>
    public static void FadeOut()
    {
        irisMaterial.mainTextureScale = irisShrinkTilling;
        irisMaterial.mainTextureOffset = irisShrinkOffset;
        irisMaterial.DOTiling(irisExpandTilling, effectSpeed).SetEase(expandEase);
        irisMaterial.DOOffset(irisExpandOffset, effectSpeed).SetEase(expandEase);
    }
}