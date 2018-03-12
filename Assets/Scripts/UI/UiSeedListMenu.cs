using HarvestValley.Managers;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace HarvestValley.Ui
{
    public class UiSeedListMenu : BuildingMenuBase<UiSeedListMenu>
    {
        //public Canvas mainCanvas;
        [SerializeField]
        private UiClickableItems scrollListItemPrefab;
        [SerializeField]
        private Transform scrollListParent;
        [SerializeField]
        private TextMeshProUGUI topInfoText;
        private Transform topInfoParentTransform;

        private List<UiClickableItems> menuItems = new List<UiClickableItems>();

        public override void Start()
        {
            CreateSeedItems();
            base.Start();
            topInfoParentTransform = topInfoText.transform.parent;
            OnDisable();
        }

        private void OnEnable()
        {
            selectedBuildingID = BuildingManager.Instance.currentSelectedBuildingID;
            selectedSourceID = BuildingManager.Instance.currentlSelectedSourceID;

            if (FieldManager.Instance == null || selectedBuildingID == -1 || selectedSourceID == -1)
            {
                return;
            }
        }

        private void OnDisable()
        {
            StopPlantingMode();
        }

        public override void AddUnlockedItemsToList()  // call on level change & game start only
        {
            base.AddUnlockedItemsToList();
        }

        private void CreateSeedItems()
        {
            for (int i = 0; i < ItemDatabase.Instance.items.Length; i++)
            {
                Item item = ItemDatabase.Instance.items[i];
                if (item != null && item.sourceID == 0)
                {
                    UiClickableItems menuItem = Instantiate(scrollListItemPrefab, scrollListParent);
                    menuItem.name = "UIItemListClick" + i;
                    menuItem.itemID = item.itemID;
                    menuItem.itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.GUI);
                    menuItem.itemName = item.name;
                    menuItem.itemCost = item.coinCost;
                    menuItem.isItemUnlocked = false;
                    menuItems.Add(menuItem);
                }
            }
        }

        public override void UpdateSeedItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (unlockedItemIDs.Contains(menuItems[i].itemID))
                {
                    menuItems[i].ItemIsUnlocked();
                }
            }
        }

        #region Planting Mode Stuff

        /// <summary>
        /// Start Planting Mode.. Callback after any plant seed is selected from the UI scroll list
        /// </summary>
        /// <param name="itemID"></param>
        public void StartPlantingMode(int itemID)
        {
            ToggleTopInfoAndUpgradeButton(true);
            topInfoText.text = "Planting " + ItemDatabase.Instance.items[itemID].name + "\n Click on the tile to plant select seed";
            FieldManager.Instance.StartPlantingMode(itemID);
            //ToggleList(false);
        }

        /// <summary>
        /// Finish planting mode
        /// </summary>
        private void StopPlantingMode()
        {
            topInfoText.text = "";
            ToggleTopInfoAndUpgradeButton(false);
            if (FieldManager.Instance != null)
            {
                FieldManager.Instance.StopPlantingMode();
            }
        }

        #endregion

        #region Toggle different UI Menus parts

        private void ToggleTopInfoAndUpgradeButton(bool flag)
        {
            if (topInfoParentTransform != null)
            {
                topInfoParentTransform.gameObject.SetActive(flag);
            }
        }

        #endregion

    }
}