public class OpenPlayerHouse : MouseUpBase
{
    public override void TouchUp()
    {
        base.TouchUp();
        MenuManager.Instance.InventoryMenuSetActive(true);
    }
}
