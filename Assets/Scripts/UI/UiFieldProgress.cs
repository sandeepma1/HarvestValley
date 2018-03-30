﻿using UnityEngine;
using TMPro;
using System;
using HarvestValley.Managers;
using HarvestValley.IO;

namespace HarvestValley.Ui
{
    public class UiFieldProgress : UiMenuBase<UiFieldProgress>
    {
        [SerializeField]
        private TextMeshProUGUI cropNameText;
        [SerializeField]
        private TextMeshProUGUI timeRemainingText;

        private int selectedFieldID = -1;
        private Item selectedItem;

        protected override void Start()
        {
            base.Start();
            Debug.Assert(cropNameText != null);
            Debug.Assert(timeRemainingText != null);
        }

        internal void EnableMenu()
        {
            if (FieldManager.Instance == null || ItemDatabase.Instance == null)
            {
                return;
            }
            selectedFieldID = FieldManager.Instance.currentSelectedBuildingID;
            selectedSourceID = FieldManager.Instance.currentlSelectedSourceID;
            selectedItem = ItemDatabase.GetItemById(FieldManager.Instance.FieldGO[selectedFieldID].itemId);
            UpdateCropName();
        }

        private void Update()
        {
            if (selectedFieldID == -1)
            {
                return;
            }
            timeRemainingText.text = TimeRemaining(FieldManager.Instance.FieldGO[selectedFieldID].dateTime);
        }

        private void UpdateCropName()
        {
            cropNameText.text = selectedItem.name;
        }
    }
}