using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiRequireItem : UiSmallItemBase
    {
        public Image itemImage;
        public TextMeshProUGUI haveCountText;
        public TextMeshProUGUI requireCountText;

        private void Awake()
        {
            ChangeUiTextColor(ref haveCountText);
            ChangeUiTextColor(ref requireCountText);
        }
    }
}