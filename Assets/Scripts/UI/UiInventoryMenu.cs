﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HarvestValley.IO;
using System;

namespace HarvestValley.Ui
{
    public class UiInventoryMenu : UiMenuBase<UiInventoryMenu>
    {
        [SerializeField]
        private UiInventoryListItem listPrefab;
        [SerializeField]
        private Transform contentParent;

        public List<UiInventoryListItem> InventoryListItems = new List<UiInventoryListItem>();

        protected override void Start()
        {
            base.Start();
            PopulateScrollListAtStart();
        }

        private void PopulateScrollListAtStart()
        {
            List<InventoryItems> playerInventory = ES2.LoadList<InventoryItems>("PlayerInventory");
            for (int i = 0; i < playerInventory.Count; i++)
            {
                AddNewOneItemInScrollList(playerInventory[i]);
            }
        }

        private void AddNewOneItemInScrollList(InventoryItems invItem)
        {
            InventoryListItems.Add(Instantiate(listPrefab, contentParent));
            InventoryListItems[InventoryListItems.Count - 1].item = invItem; // very bad code need to use dictonary or something.
        }

        public void UpdateScrollListItemCount()
        {
            for (int i = 0; i < InventoryListItems.Count; i++)
            {
                if (InventoryListItems[i].item.itemCount == 0)
                {
                    Destroy(InventoryListItems[i].gameObject);
                    InventoryListItems.RemoveAt(i);
                    Debug.LogError("Debug this need to remove list index");
                }
                else
                {
                    InventoryListItems[i].itemCountText.text = InventoryListItems[i].item.itemCount.ToString();
                }
            }
            SavePlayerInventory();
        }

        public void UpdateItems(int itemId, int itemValue)
        {
            if (itemValue == 0)
            {
                return;
            }
            bool isItemInInventory = false;
            for (int i = 0; i < InventoryListItems.Count; i++)
            {
                if (InventoryListItems[i].item.itemId == itemId)
                {
                    InventoryListItems[i].item.itemCount += itemValue;
                    //TODO: if item count is less than 0 then delete item
                    isItemInInventory = true;
                    break;
                }
            }

            if (!isItemInInventory)
            {
                AddNewOneItemInScrollList(new InventoryItems(itemId, itemValue));
            }

            UpdateScrollListItemCount();
        }

        public void RemoveItem(int itemId, int value)
        {
            for (int i = 0; i < InventoryListItems.Count; i++)
            {
                if (InventoryListItems[i].item.itemId == itemId)
                {
                    InventoryListItems[i].item.itemCount -= value;
                    break;
                }
            }
            UpdateScrollListItemCount();
        }

        public int GetItemAmountFromInventory(int itemID)
        {
            for (int i = 0; i < InventoryListItems.Count; i++)
            {
                if (InventoryListItems[i].item.itemId == itemID)
                {
                    return InventoryListItems[itemID].item.itemCount;
                }
            }
            return 0;
        }

        private void SavePlayerInventory()
        {
            List<InventoryItems> playerInventory = new List<InventoryItems>();
            for (int i = 0; i < InventoryListItems.Count; i++)
            {
                playerInventory.Add(InventoryListItems[i].item);
            }
            ES2.Save(playerInventory, "PlayerInventory");
        }
    }
}
public class InventoryItems
{
    public int itemId;
    public int itemCount;

    public InventoryItems(int i_itemId, int i_itemCount)
    {
        itemId = i_itemId;
        itemCount = i_itemCount;
    }

    public InventoryItems()
    {

    }
}