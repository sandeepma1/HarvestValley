using UnityEngine;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiSmallItemBase : MonoBehaviour
    {
        //    protected virtual void Start()
        //    {

        //    }

        protected void ChangeUiTextColor(ref TextMeshProUGUI text)
        {
            text.color = ColorConstants.NormalSecondaryText;
        }
    }
}