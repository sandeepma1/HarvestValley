using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiNeededItem : UiSmallItemBase
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TextMeshProUGUI amountText;

        public Sprite ItemImage
        {
            set
            {
                itemImage.sprite = value;
            }
        }
        public string amountNeeded
        {
            set
            {
                amountText.text = value;
            }
        }

        private void Awake()
        {
            ChangeUiTextColor(ref amountText);
        }
    }
}