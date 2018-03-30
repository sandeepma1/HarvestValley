using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiNeededItem : MonoBehaviour
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
    }
}