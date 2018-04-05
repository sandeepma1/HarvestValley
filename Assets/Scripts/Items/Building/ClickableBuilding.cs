using HarvestValley.Managers;
using HarvestValley.Ui;
using HarvestValley.IO;
using System;
using System.Collections.Generic;

public class ClickableBuilding : ClickableBase
{
    public int unlockedQueueSlots;

    private int timeCompareResult;
    internal Queue<BuildingQueue> buildingQueue = new Queue<BuildingQueue>();
    private DateTime topInQueue = DateTime.Now;
    private DateTime lastInQueue = DateTime.Now;

    public bool isProductionQueueFull = false;

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
        timeCompareResult = DateTime.Compare(topInQueue, DateTime.Now);
        if (timeCompareResult <= 0)
        {
            UiInventoryMenu.Instance.UpdateItems(buildingQueue.Peek().itemId, 1);
            buildingQueue.Dequeue();
            isProductionQueueFull = false;
            if (buildingQueue.Count != 0)
            {
                TopInQueueDateTime = buildingQueue.Peek().dateTime;
            }
            ChangedInBuilding();
            //print("item done, in queue " + buildingQueue.Count);
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

        if (isProductionQueueFull)
        {
            return;
        }

        BuildingQueue queueItem = new BuildingQueue();

        queueItem.itemId = itemIdToAdd;
        Item item = ItemDatabase.GetItemById(itemIdToAdd);

        if (buildingQueue.Count != 0)
        {
            queueItem.dateTime = lastInQueue.AddSeconds(item.timeRequiredInSeconds);
        }
        else
        {
            queueItem.dateTime = DateTime.Now.AddSeconds(item.timeRequiredInSeconds);
        }

        buildingQueue.Enqueue(queueItem);
        print("added item " + queueItem.itemId);
        lastInQueue = queueItem.dateTime;
        if (buildingQueue.Count >= unlockedQueueSlots)
        {
            isProductionQueueFull = true;
        }
        else
        {
            isProductionQueueFull = false;
        }

        if (GEM.ShowDebugInfo) print("item added, in queue " + " NOW " + DateTime.Now + " ADD " + queueItem.dateTime + " CNT " + buildingQueue.Count);
        state = BuildingState.WORKING;
        TopInQueueDateTime = buildingQueue.Peek().dateTime;
        ChangedInBuilding();
        for (int i = 0; i < item.needID.Length; i++)
        {
            if (item.needID[i] == -1)
            {
                break;
            }
            UiInventoryMenu.Instance.RemoveItem(item.needID[i], item.needAmount[i]);
        }


        //TODO: minus products required for this item not coins       
    }

    public void PopulateBuildingQueueFromSave(int[] ids, string[] dateTimes)
    {
        for (int i = 0; i < dateTimes.Length; i++)
        {
            if (ids[i] != -1)
            {
                BuildingQueue queueItem = new BuildingQueue() { itemId = ids[i], dateTime = DateTime.Parse(dateTimes[i]) };
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
        UiBuildingMenu.Instance.UpdateUiBuildingQueue();
    }
}