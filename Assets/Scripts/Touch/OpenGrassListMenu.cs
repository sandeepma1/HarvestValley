using HarvestValley.Ui;

public class OpenGrassListMenu : MouseUpBase
{
    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        MenuManager.Instance.DisplayMenu(MenuNames.GrassListMenu, MenuOpeningType.CloseAll);
    }
}