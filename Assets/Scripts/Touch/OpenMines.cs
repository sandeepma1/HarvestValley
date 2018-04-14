public class OpenMines : MouseUpBase
{
    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        MySceneManager.LoadScene("Mines");
    }
}