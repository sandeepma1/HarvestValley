using System;
using UnityEngine;
using HarvestValley.IO;

namespace HarvestValley.Ui
{
    public class UiBuyResourceMenu : UiMenuBase<UiBuyResourceMenu>
    {
        [SerializeField]
        private Transform listParent;
        [SerializeField]
        private UiNeededItem needItemPrefab;

        private UiNeededItem[] neededItems;

        protected override void Start()
        {
            base.Start();
            CreateNeededItems();
        }

        public void ShowNeededItems(InventoryItems[] items)
        {
            for (int i = 0; i < neededItems.Length; i++)
            {
                neededItems[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < items.Length; i++)
            {
                neededItems[i].gameObject.SetActive(true);
                neededItems[i].ItemImage = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemNameById(items[i].itemId), AtlasType.GUI);
                neededItems[i].amountNeeded = items[i].itemCount.ToString();
            }
        }

        private void CreateNeededItems()
        {
            neededItems = new UiNeededItem[GEM.maxRequiredItems];
            for (int i = 0; i < GEM.maxRequiredItems; i++)
            {
                neededItems[i] = Instantiate(needItemPrefab, listParent);
                neededItems[i].gameObject.SetActive(false);
            }
        }
    }
}