using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace HarvestValley.Ui
{
    public class UiClickableItems : UiSmallItemBase
    {
        public Action<int> OnClickableItemClicked;
        [SerializeField]
        private TextMeshProUGUI itemCostText;
        [SerializeField]
        private TextMeshProUGUI itemNameText;
        public Image itemImage;

        internal int itemID;
        internal string itemName;
        internal int itemCost;
        internal bool isItemUnlocked;

        private Button button;

        private void Awake()
        {
            ChangeUiTextColor(ref itemCostText);
            ChangeUiTextColor(ref itemNameText);
        }

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(ButtonClicked);
        }

        internal void UnlockItem()
        {
            isItemUnlocked = true;
            itemImage.color = ColorConstants.NormalUiItem;
            itemNameText.text = itemName;
            itemCostText.text = itemCost.ToString();
        }

        private void ButtonClicked()
        {
            if (isItemUnlocked)
            {
                OnClickableItemClicked.Invoke(itemID);
                //UiSeedListMenu.Instance.StartPlantingMode(itemID);
            }
        }
    }
}