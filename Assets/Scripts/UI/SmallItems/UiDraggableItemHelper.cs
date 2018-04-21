using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiDraggableItemHelper : UiSmallItemBase
    {
        public TextMeshProUGUI itemNameText;
        public Image itemImage;

        private void Awake()
        {
            ChangeUiTextColor(ref itemNameText);
        }
    }
}