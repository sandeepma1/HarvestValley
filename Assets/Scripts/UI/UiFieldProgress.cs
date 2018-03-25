using UnityEngine;
using TMPro;
using System;
using HarvestValley.Managers;

namespace HarvestValley.Ui
{
    public class UiFieldProgress : BuildingMenuBase<UiBuildingMenu>
    {
        [SerializeField]
        private TextMeshProUGUI cropNameText;
        [SerializeField]
        private TextMeshProUGUI timeRemainingText;

        private int selectedFieldID = -1;
        private Item selectedItem;

        public override void Start()
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
            selectedFieldID = FieldManager.Instance.currentSelectedBuildingID;
            selectedSourceID = FieldManager.Instance.currentlSelectedSourceID;
            selectedItem = ItemDatabase.Instance.items[FieldManager.Instance.FieldGO[selectedFieldID].itemId];
            UpdateCropName();
        }

        private void Update()
        {
            timeRemainingText.text = TimeRemaining(FieldManager.Instance.FieldGO[selectedFieldID].dateTime);
        }

        private void UpdateCropName()
        {
            cropNameText.text = selectedItem.name;
        }
    }
}