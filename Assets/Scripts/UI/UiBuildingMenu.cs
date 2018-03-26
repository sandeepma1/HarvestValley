using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarvestValley.Managers;
using System.Collections.Generic;

namespace HarvestValley.Ui
{
    public class UiBuildingMenu : BuildingMenuBase<UiBuildingMenu>
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

        internal Canvas mainCanvas;
        private UiDraggableItem[] menuItems; // making this array as not more than 9 items will be in building menu       
        private List<int> unlockedBuildingItemID = new List<int>();
        private int selectedBuilUnlQuSlots;
        private bool showTimer;
        BuildingQueue[] buildingQueue;

        public override void Start()
        {
            menuItems = new UiDraggableItem[GEM.maxQCount];
            CreateItems();
            //CreateQueueItems();
            base.Start();
            mainCanvas = GetComponent<Canvas>();
            unlockNewSlotButton.onClick.AddListener(UnlockNewSlotButtonPressed);
        }

        private void Update()
        {
            if (showTimer) { UpdateTimer(); }
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
            }
        }
    }
}