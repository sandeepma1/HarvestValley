using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarvestValley.Managers;
using System.Collections.Generic;
using System;

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
        private Transform queueParent;
        internal Canvas mainCanvas;

        private UiDraggableItem[] menuItems; // making this array as not more than 8 items will be in building menu
        private List<int> unlockedBuildingItemID = new List<int>();

        private const int maxItemsCount = 8;

        public override void Start()
        {
            menuItems = new UiDraggableItem[maxItemsCount];
            CreateItems();
            base.Start();
            mainCanvas = GetComponent<Canvas>();
            // OnDisable();
        }

        private void OnEnable()
        {
            selectedBuildingID = BuildingManager.Instance.currentSelectedBuildingID;
            selectedSourceID = BuildingManager.Instance.currentlSelectedSourceID;

            if (menuItems == null || BuildingManager.Instance == null || selectedBuildingID == -1 || selectedSourceID == -1)
            {
                return;
            }
            PopulateBuildingItems();
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

            for (int i = 0; i < maxItemsCount; i++)
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
            print("PopulateBuildingItems" + unlockedBuildingItemID.Count);
        }

        private void CreateItems()
        {
            for (int i = 0; i < maxItemsCount; i++)
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