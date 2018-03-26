using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiClickableItems : MonoBehaviour
    {
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
                UiSeedListMenu.Instance.StartPlantingMode(itemID);
            }
        }
    }
}