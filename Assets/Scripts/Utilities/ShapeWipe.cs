using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ShapeWipe : Singleton<ShapeWipe>
{
    static Image circleImage;

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

    public static void FadeIn()
    {
        circleImage.transform.DOScale(30, 1);
    }

    public static void FadeOut()
    {
        circleImage.transform.DOScale(0, 1);
    }
}