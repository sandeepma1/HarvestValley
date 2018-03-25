using UnityEngine;

public class ClickableBase : MouseUpBase
{
    public SpriteRenderer buildingSprite;
    public int buildingId;
    public int sourceId;
    public Vector2 pos;
    public int level;
    public BuildingState state;

    protected int itemIDToBePlaced = -1;

    private void Awake()
    {
        buildingSprite = GetComponent<SpriteRenderer>();
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
    }

    public virtual void AddItemToProductionQueue(int itemId)
    {
        //Nothing at the moment
    }
}