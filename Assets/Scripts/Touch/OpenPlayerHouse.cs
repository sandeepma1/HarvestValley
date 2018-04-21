using HarvestValley.Ui;

public class OpenPlayerHouse : MouseUpBase
{
    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        MenuManager.Instance.DisplayMenu(MenuNames.Inventory, MenuOpeningType.CloseAll);
    }
}