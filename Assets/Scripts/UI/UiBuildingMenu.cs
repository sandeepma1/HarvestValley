using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarvestValley.Managers;
using System.Collections.Generic;
using System.Collections;
using System;

namespace HarvestValley.Ui
{
    public class UiBuildingMenu : BuildingMenuBase<UiBuildingMenu>
    {
        [SerializeField]
        private UiDraggableItem uiDraggableItemPrefab;
        [SerializeField]
        private UiQueueItem uiQueueItemPrefab;
        [SerializeField]
        private TextMeshProUGUI buildingName;
        [SerializeField]
        private Image buildingImage;
        [SerializeField]
        private Transform itemParent;
        [SerializeField]
        private TextMeshProUGUI currentTimerText;
        [SerializeField]
        private TextMeshProUGUI waitingText;

        internal Canvas mainCanvas;

        private UiDraggableItem[] menuItems; // making this array as not more than 9 items will be in building menu
        [SerializeField]
        private UiQueueItem[] queueItems;
        private List<int> unlockedBuildingItemID = new List<int>();
        private ClickableBuilding currentSelectedBuilding;
        private bool showTimer;
        BuildingQueue[] buildingQueue;
        private TimeSpan remainingTime;

        public override void Start()
        {
            menuItems = new UiDraggableItem[GEM.maxQCount];
            CreateItems();
            //CreateQueueItems();
            base.Start();
            mainCanvas = GetComponent<Canvas>();
            // OnDisable();
        }

        private void Update()
        {
            if (showTimer) { UpdateTimer(); }
        }

        private void OnEnable()
        {
            if (BuildingManager.Instance == null)
            {
                selectedBuildingID = -1;
                selectedSourceID = -1;
                return;
            }

            selectedBuildingID = BuildingManager.Instance.currentSelectedBuildingID;
            selectedSourceID = BuildingManager.Instance.currentlSelectedSourceID;

            if (menuItems == null || queueItems == null || BuildingManager.Instance == null || selectedBuildingID == -1 || selectedSourceID == -1)
            {
                return;
            }
            PopulateBuildingItems();
            PopulateBuildingQueue();
            UpdateUiBuildingQueue();
        }

        public void PopulateBuildingItems()
        {
            unlockedBuildingItemID.Clear();

            for (int i = 0; i < unlockedItemIDs.Count; i++)
            {
                Item item = ItemDatabase.Instance.items[unlockedItemIDs[i]];
                if (item != null && item.sourceID == selectedSourceID)
                {
                    unlockedBuildingItemID.Add(item.itemID);
                }
            }

            for (int i = 0; i < GEM.maxQCount; i++)
            {
                menuItems[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < unlockedBuildingItemID.Count; i++)
            {
                Item item = ItemDatabase.Instance.items[unlockedBuildingItemID[i]];
                menuItems[i].gameObject.SetActive(true);
                menuItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.GUI);
                menuItems[i].ItemUnlocked();
                menuItems[i].itemID = item.itemID;
                menuItems[i].itemName = item.name;
            }
        }

        private void PopulateBuildingQueue()
        {
            currentSelectedBuilding = BuildingManager.Instance.BuildingsGO[selectedBuildingID];

            for (int i = 0; i < GEM.maxQCount; i++)
            {
                queueItems[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < currentSelectedBuilding.unlockedQueueSlots; i++)
            {
                queueItems[i].gameObject.SetActive(true);
            }
        }

        public void UpdateUiBuildingQueue()
        {
            buildingQueue = BuildingManager.Instance.BuildingsGO[selectedBuildingID].CurrentItemsInQueue();

            for (int i = 0; i < currentSelectedBuilding.unlockedQueueSlots; i++)
            {
                queueItems[i].itemImage.sprite = null;
            }

            for (int i = 0; i < buildingQueue.Length; i++)
            {
                queueItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.Instance.items[buildingQueue[i].id].slug, AtlasType.GUI);
            }

            if (buildingQueue.Length > 0)
            {
                showTimer = true;
            }
            else
            {
                showTimer = false;
                currentTimerText.text = "";
            }

            if (buildingQueue.Length > 1)
            {
                waitingText.text = "Waiting";
            }
            else
            {
                waitingText.text = "";
            }
        }

        private void UpdateTimer()
        {
            remainingTime = buildingQueue[0].dateTime.Subtract(DateTime.UtcNow);

            if (remainingTime <= new TimeSpan(360, 0, 0, 0))
            { //> 1year
                currentTimerText.text = remainingTime.Days.ToString() + "d " + remainingTime.Hours.ToString() + "h";
            }
            if (remainingTime <= new TimeSpan(1, 0, 0, 0))
            { //> 1day
                currentTimerText.text = remainingTime.Hours.ToString() + "h " + remainingTime.Minutes.ToString() + "m";
            }
            if (remainingTime <= new TimeSpan(0, 1, 0, 0))
            { //> 1hr
                currentTimerText.text = remainingTime.Minutes.ToString() + "m " + remainingTime.Seconds.ToString() + "s";
            }
            if (remainingTime <= new TimeSpan(0, 0, 1, 0))
            { // 1min
                currentTimerText.text = remainingTime.Seconds.ToString() + "s";
            }
            if (remainingTime <= new TimeSpan(0, 0, 0, 0))
            {
                currentTimerText.text = "";
            }
        }

        private void CreateItems()
        {
            for (int i = 0; i < GEM.maxQCount; i++)
            {
                menuItems[i] = Instantiate(uiDraggableItemPrefab, itemParent);
                menuItems[i].name = "DraggableUIItem" + i;
                menuItems[i].itemID = -1;
                menuItems[i].itemImage.sprite = null;
                menuItems[i].itemName = "Locked";
                menuItems[i].isItemUnlocked = false;
                menuItems[i].gameObject.SetActive(false);
            }
        }
    }
}