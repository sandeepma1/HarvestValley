public class OpenPlayerHouse : MouseUpBase
{
    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        MenuManager.Instance.InventoryMenuSetActive(true);
    }
}
