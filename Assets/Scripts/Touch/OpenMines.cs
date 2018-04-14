public class OpenMines : MouseUpBase
{
    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        SceneChanger.Instance.LoadScene("Mines");
    }
}