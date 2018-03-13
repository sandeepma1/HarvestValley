using System;
using UnityEngine;

public class ClickableBase : MouseUpBase
{
    public SpriteRenderer buildingSprite;
    public int buildingID;
    public int sourceID;
    public Vector2 pos;
    public int level;
    public BuildingState state;
    public int unlockedQueueSlots;

    public int itemID;
    public DateTime dateTime;

    protected int itemIDToBePlaced = -1;

    private void Awake()
    {
        buildingSprite = GetComponent<SpriteRenderer>();
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
    }

    public virtual void AddToProductionQueue(int itemId)
    {
        itemID = itemId;
        dateTime = UTC.time.liveDateTime.AddMinutes(ItemDatabase.Instance.items[itemId].timeRequiredInMins);
        state = BuildingState.WORKING;

        //string plantName = ItemDatabase.Instance.items[itemId].slug + "_0";
        ////plantsSprite.sprite = AtlasBank.Instance.GetSprite(plantName, AtlasType.Farming);
    }
}