using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HarvestValley.IO;

namespace HarvestValley.Ui
{
    public class UiInventoryMenu : UiMenuBase<UiInventoryMenu>
    {
        [SerializeField]
        private UiInventoryListItem listPrefab;
        [SerializeField]
        private GameObject scrollListContent;
        public List<InventoryItems> playerInventory = new List<InventoryItems>();

        List<UiInventoryListItem> listItems = new List<UiInventoryListItem>();

        protected override void Start()
        {
            base.Start();
            playerInventory = ES2.LoadList<InventoryItems>("PlayerInventory");
            PopulateScrollListAtStart();
        }

        private void PopulateScrollListAtStart()
        {
            for (int i = 0; i < playerInventory.Count; i++)
            {
                AddOneItemInScrollList(i);
            }
        }

        private void AddOneItemInScrollList(int scrollListID)
        {
            listItems.Add(Instantiate(listPrefab, scrollListContent.transform));
            listItems[scrollListID].itemCountText.text = playerInventory[scrollListID].itemCount.ToString();
            string itemSlug = ItemDatabase.GetItemById(scrollListID).slug;
            listItems[scrollListID].itemImage.sprite = AtlasBank.Instance.GetSprite(itemSlug, AtlasType.GUI);
            listItems[scrollListID].name = "InventoryListItem" + scrollListID;
        }

        public void UpdateScrollListItemCount()
        {
            for (int i = 0; i < playerInventory.Count; i++)
            {
                if (playerInventory[i].itemCount == 0)
                {
                    listItems[i].gameObject.SetActive(false);
                }
                else
                {
                    listItems[i].gameObject.SetActive(true);
                    listItems[i].itemCountText.text = playerInventory[i].itemCount.ToString();
                }
            }
            SavePlayerInventory();
        }

        public void UpdateItems(int itemId, int itemValue)
        {
            foreach (var item in playerInventory)
            {
                if (item.itemId == itemId)
                {
                    item.itemCount += itemValue;
                    break;
                }
                else
                {
                    AddNewItem(itemId, itemValue);
                    break;
                }
            }
            UpdateScrollListItemCount();
        }

        public void AddNewItem(int itemId, int itemValue)
        {
            if (itemValue == 0)
            {
                return;
            }
            playerInventory.Add(new InventoryItems(itemId, itemValue));
            AddOneItemInScrollList(playerInventory.Count - 1);
            UpdateScrollListItemCount();
        }

        public void RemoveItem(int id, int value)
        {
            playerInventory[id].itemCount -= value;
            UpdateScrollListItemCount();
        }

        private void SavePlayerInventory()
        {
            ES2.Save(playerInventory, "PlayerInventory");
        }

        public int GetItemAmountFromInventory(int itemID)
        {
            for (int i = 0; i < playerInventory.Count; i++)
            {
                if (playerInventory[i].itemId == itemID)
                {
                    return playerInventory[itemID].itemCount;
                    break;
                }
            }
            return 0;
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