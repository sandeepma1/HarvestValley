﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarvestValley.Managers;
using System.Collections.Generic;
using HarvestValley.IO;
using System;

namespace HarvestValley.Ui
{
    public class UiBuildingMenu : UiMenuBase<UiBuildingMenu>
    {
        [SerializeField]
        private UiDraggableItem uiDraggableItemPrefab;
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
        [SerializeField]
        private Button unlockNewSlotButton;
        [SerializeField]
        private TextMeshProUGUI unlockSlotGemCostText;
        [SerializeField]
        private UiQueueItem[] queueItems;

        // Require items to make item
        [SerializeField]
        private GameObject requiredItemsWindow;
        [SerializeField]
        private UiRequireItem uiRequireItemPrefab;
        [SerializeField]
        private Transform requiredListParent;
        [SerializeField]
        private TextMeshProUGUI itemNameText;
        [SerializeField]
        private TextMeshProUGUI itemTimeToMakeText;
        [SerializeField]
        private TextMeshProUGUI itemCountInInventoryText;

        internal Canvas mainCanvas;
        private UiDraggableItem[] menuItems; // making this array as not more than 9 items will be in building menu  
        private UiRequireItem[] requiredItems; // making this array as not more than 4 items will be in building menu 
        private const int maxRequiredItems = 4;
        private List<int> unlockedBuildingItemID = new List<int>();
        private int selectedBuilUnlQuSlots;
        private bool showTimer;
        BuildingQueue[] buildingQueue;


        protected override void Start()
        {
            menuItems = new UiDraggableItem[GEM.maxQCount];
            requiredItems = new UiRequireItem[maxRequiredItems];
            CreateItems();
            CreateRequiredItems();
            base.Start();
            mainCanvas = GetComponent<Canvas>();
            unlockNewSlotButton.onClick.AddListener(UnlockNewSlotButtonPressed);
            AddUnlockedItemsToList();
        }

        private void Update()
        {
            if (showTimer) { UpdateTimer(); }
        }

        public override void AddUnlockedItemsToList()  // call on level change & game start only
        {
            base.AddUnlockedItemsToList();
            PopulateBuildingItems();
        }

        internal void EnableMenu()
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
            PopulateRequiredItems();
            PopulateBuildingQueue();
            AddNewSlotForPurchase();
            UpdateUiBuildingQueue();
        }

        private void UnlockNewSlotButtonPressed()
        {
            print("Unlock new slot");
            BuildingManager.Instance.BuildingsGO[selectedBuildingID].NewQueueSlotButtonPressed();
            AddNewSlotForPurchase();
        }

        private void PopulateBuildingItems()
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
                menuItems[i].itemID = item.itemID;
                menuItems[i].itemName = item.name;
                menuItems[i].ItemUnlocked();
            }
        }

        private void PopulateRequiredItems()
        {
            for (int i = 0; i < requiredItems.Length; i++)
            {
                requiredItems[i].gameObject.SetActive(false);
            }
            requiredItemsWindow.SetActive(false);
        }

        private void PopulateBuildingQueue()
        {
            selectedBuilUnlQuSlots = BuildingManager.Instance.BuildingsGO[selectedBuildingID].unlockedQueueSlots;

            for (int i = 0; i < GEM.maxQCount; i++)
            {
                queueItems[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < selectedBuilUnlQuSlots; i++)
            {
                queueItems[i].gameObject.SetActive(true);
            }
        }

        //Showing +1 slot to unlock slot by gems
        private void AddNewSlotForPurchase()
        {
            selectedBuilUnlQuSlots = BuildingManager.Instance.BuildingsGO[selectedBuildingID].unlockedQueueSlots;

            if (selectedBuilUnlQuSlots == GEM.maxQCount)
            {
                unlockNewSlotButton.transform.SetParent(this.transform);
                unlockNewSlotButton.gameObject.SetActive(false);
            }
            else
            {
                queueItems[selectedBuilUnlQuSlots].gameObject.SetActive(true);
                unlockNewSlotButton.transform.SetParent(queueItems[selectedBuilUnlQuSlots].transform);
                unlockNewSlotButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            }
        }

        public void UpdateUiBuildingQueue()
        {
            if (selectedBuildingID == -1)
            {
                return;
            }

            buildingQueue = BuildingManager.Instance.BuildingsGO[selectedBuildingID].CurrentItemsInQueue();

            for (int i = 0; i < selectedBuilUnlQuSlots; i++)
            {
                queueItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite("Transperent", AtlasType.GUI);
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
            currentTimerText.text = TimeRemaining(buildingQueue[0].dateTime);
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
                menuItems[i].ItemClickedDragged += ItemClickedDraggedEventHandler;
            }
        }

        private void CreateRequiredItems()
        {
            for (int i = 0; i < maxRequiredItems; i++)
            {
                requiredItems[i] = Instantiate(uiRequireItemPrefab, requiredListParent);
                requiredItems[i].name = "RequiredItem" + i;
                requiredItems[i].itemImage.sprite = null;
                requiredItems[i].haveCount.text = "1";
                requiredItems[i].requireCount.text = "/2";
                requiredItems[i].gameObject.SetActive(false);
            }
        }

        private void ItemClickedDraggedEventHandler(int itemID)
        {
            requiredItemsWindow.SetActive(true);
            Item selectedItem = ItemDatabase.Instance.items[itemID];

            itemTimeToMakeText.text = SecondsToDuration((int)selectedItem.timeRequiredInSeconds);
            itemCountInInventoryText.text = UiInventoryMenu.Instance.GetItemCountFromInventory(selectedItem.itemID).ToString();
            itemNameText.text = selectedItem.name;

            for (int i = 0; i < requiredItems.Length; i++)
            {
                requiredItems[i].gameObject.SetActive(false);

                int neededItemId = selectedItem.needID[i];
                if (neededItemId == -1) { continue; }

                int neededAmount = selectedItem.needAmount[i];
                Item needItem = ItemDatabase.Instance.items[neededItemId];
                requiredItems[i].gameObject.SetActive(true);
                requiredItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite(needItem.slug, AtlasType.GUI);

                requiredItems[i].haveCount.text = UiInventoryMenu.Instance.GetItemCountFromInventory(needItem.itemID).ToString();
                requiredItems[i].requireCount.text = "/" + neededAmount.ToString();
                print(needItem.needAmount[i]);
            }

            //Item needItem1 = ItemDatabase.Instance.items[item.needID1];
            //requiredItems[0].gameObject.SetActive(true);
            //requiredItems[0].itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.GUI);
            //requiredItems[0].haveCount.text = UiInventoryMenu.Instance.GetItemCountFromInventory(needItem1.itemID).ToString();
            //requiredItems[0].requireCount.text = "/" + needItem1.needAmount1.ToString();

        }
    }
}