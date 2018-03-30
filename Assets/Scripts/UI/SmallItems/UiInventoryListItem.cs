using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiInventoryListItem : UiSmallItemBase
    {
        public Image itemImage;
        public TextMeshProUGUI itemCountText;

        private void Awake()
        {
            ChangeUiTextColor(ref itemCountText);
        }
    }
}