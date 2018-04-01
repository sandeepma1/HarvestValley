using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarvestValley.IO;

namespace HarvestValley.Ui
{
    public class UiInventoryListItem : UiSmallItemBase
    {
        public InventoryItems item = new InventoryItems();
        public Image itemImage;
        public TextMeshProUGUI itemCountText;

        private void Awake()
        {
            ChangeUiTextColor(ref itemCountText);
        }

        private void Start()
        {
            string itemSlug = ItemDatabase.GetItemById(item.itemId).slug;

            itemImage.sprite = AtlasBank.Instance.GetSprite(itemSlug, AtlasType.GUI);
            itemCountText.text = item.itemCount.ToString();
        }
    }
}