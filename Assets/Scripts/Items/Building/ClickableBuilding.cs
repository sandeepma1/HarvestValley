using HarvestValley.Managers;
using System;
using HarvestValley.Ui;
using System.Collections.Generic;

public class ClickableBuilding : ClickableBase
{
    public int[] itemId;
    public DateTime[] dateTime;
    public int unlockedQueueSlots;

    private Queue<BuildingQueue> buildingQueue = new Queue<BuildingQueue>();

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        BuildingManager.Instance.OnBuildingClicked(buildingId, sourceId);
    }

    public override void AddToProductionQueue(int itemIdToAdd)
    {
        base.AddToProductionQueue(itemIdToAdd);
        if (buildingQueue.Count >= unlockedQueueSlots)
        {
            print("Queue slots are full unlock slots by gems!!");//TODO: Gems window popup
            return;
        }
        BuildingQueue queueItem = new BuildingQueue()
        {
            id = itemIdToAdd,
            dateTime = UTC.time.liveDateTime.AddMinutes(ItemDatabase.Instance.items[itemIdToAdd].timeRequiredInMins)
        };

        buildingQueue.Enqueue(queueItem);
        print("item added");
        state = BuildingState.WORKING;
        //TODO: minus products required for this item not coins       
    }

    public void PopulateBuildingQueueFromSave(int[] ids, string[] dateTime)
    {
        for (int i = 0; i < ids.Length; i++)
        {
            BuildingQueue queueItem = new BuildingQueue() { id = ids[i], dateTime = DateTime.Parse(dateTime[i]) };
            buildingQueue.Enqueue(queueItem);
        }
    }

    public BuildingQueue[] CurrentItemsInQueue()
    {
        return buildingQueue.ToArray();
    }
}