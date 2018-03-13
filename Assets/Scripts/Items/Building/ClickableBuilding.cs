using HarvestValley.Managers;
using System;
using HarvestValley.Ui;

public class ClickableBuilding : ClickableBase
{
    public int[] itemId;
    public DateTime[] dateTime;
    public int unlockedQueueSlots;

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        BuildingManager.Instance.OnBuildingClicked(buildingId, sourceId);
    }

    public override void AddToProductionQueue(int itemIdToAdd) // Todo: write better code :(
    {
        base.AddToProductionQueue(itemIdToAdd);

        for (int i = 0; i < itemId.Length; i++)
        {
            if (i > unlockedQueueSlots)
            {
                print("Queue slots are full or");
                break;
            }
            if (itemId[i] == -1)
            {
                itemId[i] = itemIdToAdd;
                dateTime[i] = UTC.time.liveDateTime.AddMinutes(ItemDatabase.Instance.items[itemIdToAdd].timeRequiredInMins);
                UiBuildingMenu.Instance.AddNewQueueItem(itemIdToAdd, i);
                break;
            }
            else
            {
                print("Queue slots are full");
            }
        }
        state = BuildingState.WORKING;

        //TODO: minus products required for this item not coins
    }
}