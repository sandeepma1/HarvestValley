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
        public List<FarmItems> playerInventory = new List<FarmItems>();

        List<UiInventoryListItem> listItems = new List<UiInventoryListItem>();

        protected override void Start()
        {
            base.Start();
            playerInventory = ES2.LoadList<FarmItems>("PlayerInventory");
            PopulateScrollListAtStart();
            InvokeRepeating("SavePlayerInventory", 3, 3);
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
            listItems[scrollListID].itemCountText.text = playerInventory[scrollListID].count.ToString();
            string itemSlug = ItemDatabase.GetItemById(scrollListID).slug;
            listItems[scrollListID].itemImage.sprite = AtlasBank.Instance.GetSprite(itemSlug, AtlasType.GUI);
            listItems[scrollListID].name = "InventoryListItem" + scrollListID;
        }

        public void UpdateScrollListItemCount()
        {
            for (int i = 0; i < playerInventory.Count; i++)
            {
                listItems[i].itemCountText.text = playerInventory[i].count.ToString();
            }
        }

        public void UpdateFarmItems(int id, int value)
        {
            foreach (var item in playerInventory)
            {
                if (item.id == id)
                {
                    item.count += value;
                }
            }
            UpdateScrollListItemCount();
        }

        public void AddNewFarmItem(int id, int value)
        {
            playerInventory.Add(new FarmItems(id, value));
            AddOneItemInScrollList(playerInventory.Count - 1);
            UpdateScrollListItemCount();
        }

        private void SavePlayerInventory()
        {
            ES2.Save(playerInventory, "PlayerInventory");
        }

        public int GetItemCountFromInventory(int itemID)
        {
            return playerInventory[itemID].count;
        }
    }
}
public class FarmItems
{
    public int id;
    public int count;

    public FarmItems(int i_id, int i_count)
    {
        id = i_id;
        count = i_count;
    }

    public FarmItems()
    {

    }
}