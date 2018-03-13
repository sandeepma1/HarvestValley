using HarvestValley.Managers;

public class ClickableBuilding : ClickableBase
{
    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        BuildingManager.Instance.OnBuildingClicked(buildingID, sourceID);
    }

    public override void AddToProductionQueue(int itemId)
    {
        base.AddToProductionQueue(itemId);
        //TODO: minus products required for this item not coins
    }
}