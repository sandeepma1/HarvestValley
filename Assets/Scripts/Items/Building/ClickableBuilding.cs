using HarvestValley.Managers;
using HarvestValley.Ui;
using HarvestValley.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ClickableBuilding : ClickableBase
{
    private Transform producedItemParent;
    public Queue<int> producedItemIdList;
    public Queue<GameObject> producedItemGO = new Queue<GameObject>();
    public int unlockedQueueSlots;
    private int timeCompareResult;
    internal Queue<BuildingQueue> buildingQueue = new Queue<BuildingQueue>();
    private DateTime topInQueue = DateTime.Now;
    private DateTime lastInQueue = DateTime.Now;
    private GameObject producedItemPrefab;
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

    private void Start()
    {
        producedItemParent = transform.GetChild(0); // Todo: Carefuly about this
        producedItemPrefab = Resources.Load("ProducedItem") as GameObject;
        if (producedItemIdList != null)
        {
            foreach (var item in producedItemIdList.Reverse())
            {
                AddNewProducedItem(item);
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
            ItemProductionCompleted();
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

    private void ItemProductionCompleted()
    {
        producedItemIdList.Enqueue(buildingQueue.Peek().itemId);
        AddNewProducedItem(producedItemIdList.Peek());
    }

    private void AddNewProducedItem(int itemId)
    {
        GameObject itemProduced = Instantiate(producedItemPrefab, producedItemParent);
        itemProduced.GetComponent<SpriteRenderer>().sprite = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemSlugById(itemId), AtlasType.GUI);
        producedItemGO.Enqueue(itemProduced);
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        if (producedItemIdList.Count == 0)
        {
            BuildingManager.Instance.OnBuildingClicked(buildingId, sourceId);
        }
        else
        {
            //Todo: check if inventory is not full;=
            UiInventoryMenu.Instance.UpdateItems(producedItemIdList.Peek(), 1);
            producedItemIdList.Dequeue();
            Destroy(producedItemGO.Peek());
            producedItemGO.Dequeue();
        }
    }

    public override void AddItemToProductionQueue(int itemIdToAdd)
    {
        base.AddItemToProductionQueue(itemIdToAdd);

        if (isProductionQueueFull)
        {
            print("This will never trigger");//Todo remove if never trigger
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