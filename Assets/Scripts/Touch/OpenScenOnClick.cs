using UnityEngine;

public class OpenScenOnClick : MouseUpBase
{
    [SerializeField]
    private string sceneName;

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        SceneChanger.Instance.LoadScene(sceneName);
    }
}