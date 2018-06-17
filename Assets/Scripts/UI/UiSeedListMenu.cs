using HarvestValley.Managers;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using HarvestValley.IO;
using System;
using UnityEngine.UI;

namespace HarvestValley.Ui
{
    public class UiSeedListMenu : UiMenuBase<UiSeedListMenu>
    {
        //public Canvas mainCanvas;
        [SerializeField]
        private GameObject seedListWindow;
        [SerializeField]
        private UiClickableItems scrollListItemPrefab;
        [SerializeField]
        private Transform scrollListParent;
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private TextMeshProUGUI seedNameText;
        [SerializeField]
        private TextMeshProUGUI descriptionText;
        [SerializeField]
        private TextMeshProUGUI durationText;
        [SerializeField]
        private TextMeshProUGUI yieldText;
        [SerializeField]
        private Button plantButton;

        private int selectedSeedID = -1;

        private string topMessage;

        private List<UiClickableItems> menuItems = new List<UiClickableItems>();

        protected override void Start()
        {
            CreateSeedItems();
            base.Start();
            AddUnlockedItemsToList();
            closeButton.onClick.AddListener(CloseButtonPressed);
            plantButton.onClick.AddListener(PlantButtonClicked);
            ClearSeedDescription();
        }

        private void OnEnable()
        {
            seedListWindow.SetActive(true);
        }

        public override void AddUnlockedItemsToList()  // call on level change & game start only
        {
            base.AddUnlockedItemsToList();
            UpdateSeedItems();
        }

        private void CreateSeedItems()
        {
            for (int i = 0; i < ItemDatabase.GetItemslength(); i++)
            {
                Item item = ItemDatabase.GetItemById(i);
                if (item != null && item.sourceID == 0)
                {
                    UiClickableItems menuItem = Instantiate(scrollListItemPrefab, scrollListParent);
                    menuItem.name = "UIItemListClick" + i;
                    menuItem.itemID = item.itemID;
                    menuItem.itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.GUI);
                    menuItem.itemName = item.name;
                    menuItem.itemCost = item.coinCost;
                    menuItem.isItemUnlocked = false;
                    menuItem.OnClickableItemClicked += OnClickableItemClickedEventHandler;
                    menuItems.Add(menuItem);
                }
            }
        }

        public void UpdateSeedItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (allUnlockedItemIDs.Contains(menuItems[i].itemID))
                {
                    menuItems[i].UnlockItem();
                }
            }
        }

        private void CloseButtonPressed()
        {
            StopPlantingMode();
        }

        #region Planting Mode Stuff

        private void PlantButtonClicked()
        {
            UiMessage.Instance.SetTopMessage(topMessage);
            ClearSeedDescription();
            MenuManager.Instance.CloseMenu();
            selectedSeedID = -1;
        }

        /// <summary>
        /// Start Planting Mode.. Callback after any plant seed is selected from the UI scroll list
        /// </summary>
        /// <param name="itemID"></param>
        public void OnClickableItemClickedEventHandler(int itemID)
        {
            selectedSeedID = itemID;
            topMessage = "Planting " + ItemDatabase.GetItemById(itemID).name + "\n Walk around the field to plant select seed";
            FieldManager.Instance.StartPlantingMode(itemID);
            FillSeedDescription(itemID);
        }

        private void FillSeedDescription(int itemID)
        {
            plantButton.gameObject.SetActive(true);
            Item selectedSeed = ItemDatabase.GetItemById(itemID);
            seedNameText.text = selectedSeed.name;
            descriptionText.text = selectedSeed.description;
            durationText.text = "Duration: " + selectedSeed.timeRequiredInSeconds;
            yieldText.text = "Base Yield: " + selectedSeed.baseYieldMin + " - " + selectedSeed.baseYieldMax;
        }

        private void ClearSeedDescription()
        {
            plantButton.gameObject.SetActive(false);
            seedNameText.text = "";
            descriptionText.text = "";
            durationText.text = "";
            yieldText.text = "";
        }

        /// <summary>
        /// Finish planting mode
        /// </summary>
        internal void StopPlantingMode()
        {
            UiMessage.Instance.HideClearTopMessageText();
            if (FieldManager.Instance != null)
            {
                FieldManager.Instance.StopPlantingMode();
            }
        }

        #endregion
    }
}