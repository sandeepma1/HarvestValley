using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarvestValley.Managers;
using System.Collections.Generic;
using System.Collections;

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

        public override void Start()
        {
            menuItems = new UiDraggableItem[GEM.maxQCount];
            CreateItems();
            //CreateQueueItems();
            base.Start();
            mainCanvas = GetComponent<Canvas>();
            // OnDisable();
        }

        private void OnEnable()
        {
            selectedBuildingID = BuildingManager.Instance.currentSelectedBuildingID;
            selectedSourceID = BuildingManager.Instance.currentlSelectedSourceID;

            if (menuItems == null || queueItems == null || BuildingManager.Instance == null || selectedBuildingID == -1 || selectedSourceID == -1)
            {
                return;
            }
            PopulateBuildingItems();
            PopulateBuildingQueue();
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

        public void AddNewQueueItem(int itemIdToAdd, int position)
        {
            queueItems[position].itemImage.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.Instance.items[itemIdToAdd].slug, AtlasType.GUI);
            if (position == 0)
            {
                currentTimerText.text = currentSelectedBuilding.dateTime[0].ToString();
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