using HarvestValley.Managers;
using HarvestValley.Ui;
using System;
using System.Collections.Generic;

public class ClickableBuilding : ClickableBase
{
    public int unlockedQueueSlots;

    private int timeCompareResult;
    internal Queue<BuildingQueue> buildingQueue = new Queue<BuildingQueue>();
    private DateTime topInQueue = DateTime.UtcNow;
    private DateTime lastInQueue = DateTime.UtcNow;

    private DateTime TopInQueueDateTime
    {
        get
        {
            return topInQueue;
        }
        set
        {
            if (buildingQueue.Count != 0)
            {
                topInQueue = value;
            }
        }
    }

    private void Update()
    {
        if (buildingQueue.Count <= 0)
        {
            return;
        }
        timeCompareResult = DateTime.Compare(topInQueue, DateTime.UtcNow);
        if (timeCompareResult <= 0)
        {
            buildingQueue.Dequeue();
            if (buildingQueue.Count != 0)
            {
                TopInQueueDateTime = buildingQueue.Peek().dateTime;
            }
            ChangedInBuilding();
            print("item done, in queue " + buildingQueue.Count);
        }
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        BuildingManager.Instance.OnBuildingClicked(buildingId, sourceId);
    }

    public override void AddItemToProductionQueue(int itemIdToAdd)
    {
        base.AddItemToProductionQueue(itemIdToAdd);

        if (buildingQueue.Count >= unlockedQueueSlots)
        {
            print("Queue slots are full unlock slots by gems!!");//TODO: Gems window popup
            return;
        }

        BuildingQueue queueItem = new BuildingQueue();

        queueItem.id = itemIdToAdd;

        if (buildingQueue.Count != 0)
        {
            queueItem.dateTime = lastInQueue.AddSeconds(ItemDatabase.Instance.items[itemIdToAdd].timeRequiredInSeconds);
        }
        else
        {
            queueItem.dateTime = DateTime.UtcNow.AddSeconds(ItemDatabase.Instance.items[itemIdToAdd].timeRequiredInSeconds);
        }

        buildingQueue.Enqueue(queueItem);
        lastInQueue = queueItem.dateTime;
        print("item added, in queue " + " NOW " + DateTime.UtcNow + " ADD " + queueItem.dateTime + " CNT " + buildingQueue.Count);
        state = BuildingState.WORKING;
        TopInQueueDateTime = buildingQueue.Peek().dateTime;
        ChangedInBuilding();
        //TODO: minus products required for this item not coins       
    }

    public void PopulateBuildingQueueFromSave(int[] ids, string[] dateTimes)
    {
        for (int i = 0; i < dateTimes.Length; i++)
        {
            if (ids[i] != -1)
            {
                BuildingQueue queueItem = new BuildingQueue() { id = ids[i], dateTime = DateTime.Parse(dateTimes[i]) };
                buildingQueue.Enqueue(queueItem);
            }
        }
        if (buildingQueue.Count != 0)
        {
            TopInQueueDateTime = buildingQueue.Peek().dateTime;
        }
    }

    public BuildingQueue[] CurrentItemsInQueue()
    {
        return buildingQueue.ToArray();
    }

    public void NewQueueSlotButtonPressed()
    {
        unlockedQueueSlots++;
        ChangedInBuilding();
    }

    private void ChangedInBuilding()
    {
        BuildingManager.Instance.SaveBuildings();
        if (UiBuildingMenu.Instance.isActiveAndEnabled)
        {
            UiBuildingMenu.Instance.UpdateUiBuildingQueue();
        }
    }
}