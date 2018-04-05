using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarvestValley.Managers;
using System.Collections.Generic;
using HarvestValley.IO;

namespace HarvestValley.Ui
{
    public class UiBuildingMenu : UiMenuBase<UiBuildingMenu>
    {
        [SerializeField]
        private UiDraggableItem uiDraggableItemPrefab;
        [SerializeField]
        private TextMeshProUGUI buildingNameText;
        [SerializeField]
        private Image buildingImage;
        [SerializeField]
        private Transform itemScrollListContentTransform;
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

        private List<int> unlockedThisBuildingItemIds = new List<int>();
        private int selectedBuilUnlQuSlots;
        private bool showTimer;
        BuildingQueue[] buildingQueue;

        protected override void Start()
        {
            menuItems = new UiDraggableItem[GEM.maxBuildingQueueCount];
            requiredItems = new UiRequireItem[GEM.maxRequiredItems];
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
            PopulateBuildingImageName();
            PopulateBuildingItems();
            AddNextLockedItem();
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

        private void PopulateBuildingImageName()
        {
            string buildingName = SourceDatabase.GetSourceInfoById(selectedSourceID).slug;
            buildingNameText.text = SourceDatabase.GetSourceInfoById(selectedSourceID).name;
            buildingImage.sprite = AtlasBank.Instance.GetSprite(buildingName, AtlasType.Buildings);
        }

        private void PopulateBuildingItems()
        {
            unlockedThisBuildingItemIds.Clear();

            for (int i = 0; i < allUnlockedItemIDs.Count; i++)
            {
                Item item = ItemDatabase.GetItemById(allUnlockedItemIDs[i]);
                if (item != null && item.sourceID == selectedSourceID)
                {
                    unlockedThisBuildingItemIds.Add(item.itemID);
                }
            }

            for (int i = 0; i < GEM.maxBuildingQueueCount; i++)
            {
                menuItems[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < unlockedThisBuildingItemIds.Count; i++)
            {
                Item item = ItemDatabase.GetItemById(unlockedThisBuildingItemIds[i]);
                menuItems[i].gameObject.SetActive(true);
                menuItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.GUI);
                menuItems[i].itemID = item.itemID;
                menuItems[i].itemName = item.name;
                menuItems[i].ItemUnlocked();
            }
        }

        private void AddNextLockedItem()
        {
            Item[] allItemsForThisBuilding = ItemDatabase.GetAllItemsBySourceId(selectedSourceID);

            //Unlocked all items for this building
            if (unlockedThisBuildingItemIds.Count + 1 > allItemsForThisBuilding.Length)
            {
                return;
            }

            Item nextLockedItem = allItemsForThisBuilding[unlockedThisBuildingItemIds.Count];

            if (unlockedThisBuildingItemIds.Count <= allItemsForThisBuilding.Length)
            {
                menuItems[unlockedThisBuildingItemIds.Count].gameObject.SetActive(true);
                menuItems[unlockedThisBuildingItemIds.Count].itemImage.sprite = AtlasBank.Instance.GetSprite(nextLockedItem.slug, AtlasType.GUI);
                menuItems[unlockedThisBuildingItemIds.Count].itemID = nextLockedItem.itemID;
                menuItems[unlockedThisBuildingItemIds.Count].itemName = nextLockedItem.name;
                menuItems[unlockedThisBuildingItemIds.Count].ItemLocked();
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

            for (int i = 0; i < GEM.maxBuildingQueueCount; i++)
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

            if (selectedBuilUnlQuSlots == GEM.maxBuildingQueueCount)
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
                queueItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemById(buildingQueue[i].itemId).slug, AtlasType.GUI);
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
            for (int i = 0; i < GEM.maxBuildingQueueCount; i++)
            {
                menuItems[i] = Instantiate(uiDraggableItemPrefab, itemScrollListContentTransform);
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
            for (int i = 0; i < GEM.maxRequiredItems; i++)
            {
                requiredItems[i] = Instantiate(uiRequireItemPrefab, requiredListParent);
                requiredItems[i].name = "RequiredItem" + i;
                requiredItems[i].itemImage.sprite = null;
                requiredItems[i].haveCountText.text = "1";
                requiredItems[i].requireCountText.text = "/2";
                requiredItems[i].gameObject.SetActive(false);
            }
        }

        private void ItemClickedDraggedEventHandler(int itemID)
        {
            if (itemID == -1)
            {
                requiredItemsWindow.SetActive(false);
            }
            else
            {
                UpdateRequiredItemsSection(itemID);
            }
        }

        private void UpdateRequiredItemsSection(int itemID)
        {
            requiredItemsWindow.SetActive(true);
            Item selectedItem = ItemDatabase.GetItemById(itemID);

            itemTimeToMakeText.text = SecondsToDuration((int)selectedItem.timeRequiredInSeconds);
            itemCountInInventoryText.text = UiInventoryMenu.Instance.GetItemAmountFromInventory(selectedItem.itemID).ToString();
            itemNameText.text = selectedItem.name;

            for (int i = 0; i < requiredItems.Length; i++)
            {
                requiredItems[i].gameObject.SetActive(false);

                int neededItemId = selectedItem.needID[i];
                if (neededItemId == -1) { continue; }

                int neededAmount = selectedItem.needAmount[i];
                Item needItem = ItemDatabase.GetItemById(neededItemId);
                requiredItems[i].gameObject.SetActive(true);
                requiredItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite(needItem.slug, AtlasType.GUI);

                int itemAmountInInventory = UiInventoryMenu.Instance.GetItemAmountFromInventory(needItem.itemID);
                requiredItems[i].haveCountText.text = itemAmountInInventory.ToString();
                requiredItems[i].haveCountText.color = (itemAmountInInventory < neededAmount) ? ColorConstants.InsufficientItemAmount : ColorConstants.NormalSecondaryText;
                requiredItems[i].requireCountText.text = "/" + neededAmount.ToString();
            }
        }

        public void ItemDroppedInZone(int itemId)
        {
            // Return if production is full
            if (BuildingManager.Instance.BuildingsGO[selectedBuildingID].isProductionQueueFull)
            {
                return;
            }
            List<InventoryItems> itemsNeeded = CheckForItemsNeededInInventory(itemId);

            if (itemsNeeded.Count == 0) // Add to production queue
            {
                BuildingManager.Instance.BuildingsGO[selectedBuildingID].AddItemToProductionQueue(itemId);
                UpdateRequiredItemsSection(itemId);
            }
            else
            {
                //dont have items, SHOW RESOURVE NEEDED MENU
                MenuManager.Instance.DisplayMenu(MenuNames.BuyResourcesMenu, MenuOpeningType.OnTop);
                UiBuyResourceMenu.Instance.ShowNeededItems(itemsNeeded.ToArray());
            }
        }

        private List<InventoryItems> CheckForItemsNeededInInventory(int itemId)
        {
            List<InventoryItems> requiredItems = new List<InventoryItems>();
            Item item = ItemDatabase.GetItemById(itemId);
            for (int i = 0; i < item.needID.Length; i++)
            {
                if (item.needID[i] == -1) { return requiredItems; }   //Return as next items will be none                    

                int itemsAmountInInventory = UiInventoryMenu.Instance.GetItemAmountFromInventory(item.needID[i]);
                if (item.needAmount[i] > itemsAmountInInventory)
                {
                    requiredItems.Add(new InventoryItems(item.needID[i], item.needAmount[i] - itemsAmountInInventory));
                }
            }
            return requiredItems;
        }
    }
}