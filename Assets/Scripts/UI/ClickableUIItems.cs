using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HarvestValley.Ui
{
    public class ClickableUIItems : MonoBehaviour
    {
        public int itemID;
        public TextMeshProUGUI itemCostText;
        public TextMeshProUGUI itemNameText;
        public Image itemImage;
        internal bool isItemUnlocked;
        private Button button;

        // Use this for initialization
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(ButtonClicked);
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