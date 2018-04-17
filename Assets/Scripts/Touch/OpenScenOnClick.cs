using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
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