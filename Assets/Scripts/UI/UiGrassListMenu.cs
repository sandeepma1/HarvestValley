﻿using HarvestValley.Managers;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using HarvestValley.IO;
using System;

namespace HarvestValley.Ui
{
    public class UiGrassListMenu : UiMenuBase<UiGrassListMenu>
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
            CreateGrassItems();
            base.Start();
            AddUnlockedItemsToList();
        }

        private void CreateGrassItems()
        {
            for (int i = 0; i < ItemDatabase.GetItemslength(); i++)
            {
                Item item = ItemDatabase.GetItemById(i);
                if (item != null && item.sourceID == 1)
                {
                    UiClickableItems menuItem = Instantiate(scrollListItemPrefab, scrollListParent);
                    menuItem.name = "UIItemListClick" + i;
                    menuItem.itemID = item.itemID;
                    menuItem.itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.Lifestock);
                    menuItem.itemName = item.name;
                    menuItem.itemCost = item.coinCost;
                    menuItem.isItemUnlocked = false;
                    menuItem.OnClickableItemClicked += StartPlantingMode;
                    menuItems.Add(menuItem);
                }
            }
        }

        public override void AddUnlockedItemsToList()  // call on level change & game start only
        {
            base.AddUnlockedItemsToList();
            UpdateGrassItems();
        }

        public void UpdateGrassItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (allUnlockedItemIDs.Contains(menuItems[i].itemID))
                {
                    menuItems[i].UnlockItem();
                }
            }
        }

        private void ToggleTopInfoAndUpgradeButton(bool flag)
        {
            topInfoParentTransform.gameObject.SetActive(flag);
        }

        #region Planting Mode Stuff

        // Start Planting Mode.. Callback after any plant seed is selected from the UI scroll list       
        public void StartPlantingMode(int itemID)
        {
            ToggleTopInfoAndUpgradeButton(true);
            topInfoText.text = "Planting " + ItemDatabase.GetItemById(itemID).name + "\n Click and drag on the ground to add grass";
            GrassLandManager.Instance.StartPlantingMode();
        }

        // Finish planting mode
        internal void StopPlantingMode()
        {
            topInfoText.text = "";
            ToggleTopInfoAndUpgradeButton(false);
            if (GrassLandManager.Instance != null)
            {
                GrassLandManager.Instance.StopPlantingMode();
            }
        }

        #endregion
    }
}