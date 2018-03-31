using HarvestValley.Managers;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using HarvestValley.IO;
using System;

namespace HarvestValley.Ui
{
    public class UiSeedListMenu : UiMenuBase<UiSeedListMenu>
    {
        //public Canvas mainCanvas;
        [SerializeField]
        private UiClickableItems scrollListItemPrefab;
        [SerializeField]
        private Transform scrollListParent;
        [SerializeField]
        private TextMeshProUGUI topInfoText;
        [SerializeField]
        private Transform topInfoParentTransform;
        private List<UiClickableItems> menuItems = new List<UiClickableItems>();

        protected override void Start()
        {
            CreateSeedItems();
            base.Start();
            AddUnlockedItemsToList();
        }

        public override void AddUnlockedItemsToList()  // call on level change & game start only
        {
            base.AddUnlockedItemsToList();
            UpdateSeedItems();
        }

        private void CreateSeedItems()
        {
            for (int i = 0; i < ItemDatabase.GetItemslength(); i++)
            {
                Item item = ItemDatabase.GetItemById(i);
                if (item != null && item.sourceID == 0)
                {
                    UiClickableItems menuItem = Instantiate(scrollListItemPrefab, scrollListParent);
                    menuItem.name = "UIItemListClick" + i;
                    menuItem.itemID = item.itemID;
                    menuItem.itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.GUI);
                    menuItem.itemName = item.name;
                    menuItem.itemCost = item.coinCost;
                    menuItem.isItemUnlocked = false;
                    menuItem.OnClickableItemClicked += StartPlantingMode;
                    menuItems.Add(menuItem);
                }
            }
        }

        public void UpdateSeedItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (allUnlockedItemIDs.Contains(menuItems[i].itemID))
                {
                    menuItems[i].UnlockItem();
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
            topInfoText.text = "Planting " + ItemDatabase.GetItemById(itemID).name + "\n Click on the tile to plant select seed";
            FieldManager.Instance.StartPlantingMode(itemID);
            //ToggleList(false);
        }

        /// <summary>
        /// Finish planting mode
        /// </summary>
        internal void StopPlantingMode()
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
            topInfoParentTransform.gameObject.SetActive(flag);
        }

        #endregion
    }
}