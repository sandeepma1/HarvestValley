using UnityEngine;
using TMPro;
using System;

namespace Hv.Ui
{
    public class UiFieldProgress : Singleton<UiFieldProgress>
    {
        [SerializeField]
        private TextMeshProUGUI cropNameText;
        [SerializeField]
        private TextMeshProUGUI timeRemainingText;

        private int selectedBuildingID = -1;
        private int selectedSourceID = -1;
        private TimeSpan remainingTime;
        private Item selectedItem;

        private void Start()
        {
            Debug.Assert(cropNameText != null);
            Debug.Assert(timeRemainingText != null);
        }

        private void OnEnable()
        {
            if (FieldManager.Instance == null)
            {
                return;
            }
            selectedBuildingID = FieldManager.Instance.currentSelectedFieldID;
            selectedSourceID = FieldManager.Instance.currentlSelectedSourceID;
            selectedItem = ItemDatabase.Instance.items[FieldManager.Instance.FieldGO[selectedBuildingID].itemID];
            UpdateCropName();
        }

        private void Update()
        {
            remainingTime = FieldManager.Instance.FieldGO[selectedBuildingID].dateTime.Subtract(UTC.time.liveDateTime);

            if (remainingTime <= new System.TimeSpan(360, 0, 0, 0))
            { //> 1year
                timeRemainingText.text = remainingTime.Days.ToString() + "d " + remainingTime.Hours.ToString() + "h";
            }
            if (remainingTime <= new System.TimeSpan(1, 0, 0, 0))
            { //> 1day
                timeRemainingText.text = remainingTime.Hours.ToString() + "h " + remainingTime.Minutes.ToString() + "m";
            }
            if (remainingTime <= new System.TimeSpan(0, 1, 0, 0))
            { //> 1hr
                timeRemainingText.text = remainingTime.Minutes.ToString() + "m " + remainingTime.Seconds.ToString() + "s";
            }
            if (remainingTime <= new System.TimeSpan(0, 0, 1, 0))
            { // 1min
                timeRemainingText.text = remainingTime.Seconds.ToString() + "s";
            }
            if (remainingTime <= new System.TimeSpan(0, 0, 0, 0))
            { // 1min
                MenuManager.Instance.CloseAllMenu();
            }
        }

        private void UpdateCropName()
        {
            cropNameText.text = selectedItem.name;
        }
    }
}