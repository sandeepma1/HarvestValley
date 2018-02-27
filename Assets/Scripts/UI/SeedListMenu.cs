using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Hv.Ui
{
    public class SeedListMenu : Singleton<SeedListMenu>
    {
        //public Canvas mainCanvas;
        [SerializeField]
        private ClickableUIItems scrollListItemPrefab;
        [SerializeField]
        private Transform scrollListParent;
        [SerializeField]
        private TextMeshProUGUI topInfoText;

        private Transform topInfoParentTransform;

        private int selectedFieldID = -1;
        private int selectedSourceID = -1;
        private ClickableUIItems[] menuItems = new ClickableUIItems[12];
        private List<int> unlockedItemIDs = new List<int>();

        private void Start()
        {
            topInfoParentTransform = topInfoText.transform.parent;
            CheckForUnlockedItems();
            OnDisable();
        }

        private void OnEnable()
        {
            if (FieldManager.Instance == null)
            {
                return;
            }
            selectedFieldID = FieldManager.Instance.currentSelectedFieldID;
            selectedSourceID = FieldManager.Instance.currentlSelectedSourceID;

            if (selectedFieldID == -1 || selectedSourceID == -1)
            {
                Debug.LogError("Selected field is -1");
                return;
            }
        }

        private void OnDisable()
        {
            StopPlantingMode();
        }

        private void InitScrollListItems()
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                Item item = ItemDatabase.Instance.items[i];

                if (item.sourceID != 0) // Checks if it is a field
                {
                    continue;
                }

                if (menuItems[i] == null)
                {
                    menuItems[i] = (Instantiate(scrollListItemPrefab, scrollListParent));
                    menuItems[i].name = "UIItemListClick" + i;
                }

                menuItems[i].itemID = item.itemID;
                menuItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.GUI);

                // TODO: may be expensive step, try to optimize            
                if (unlockedItemIDs.Contains(item.itemID)) // Unlocked
                {
                    menuItems[i].itemImage.color = ColorConstants.white;
                    menuItems[i].isItemUnlocked = true;
                    menuItems[i].itemNameText.text = item.name;
                    menuItems[i].itemCostText.text = item.coinCost.ToString();
                }
                else                                      //Locked
                {
                    menuItems[i].itemImage.color = ColorConstants.dehighlightedUiItem;
                    menuItems[i].isItemUnlocked = false;
                    menuItems[i].itemNameText.text = "Locked";
                    menuItems[i].itemCostText.text = "";
                }
            }
        }

        public void CheckForUnlockedItems()  // call on level change & game start only
        {
            unlockedItemIDs.Clear();
            for (int i = 0; i <= PlayerProfileManager.Instance.CurrentPlayerLevel(); i++)
            {
                int unlockedId = LevelUpDatabase.Instance.gameLevels[i].itemUnlockID;

                if (unlockedId >= 0)
                {
                    if (ItemDatabase.Instance.items[unlockedId].sourceID == 0) // Checks if it is a field
                    {
                        unlockedItemIDs.Add(unlockedId);
                    }
                }
            }
            InitScrollListItems();
        }

        #region Planting Mode Stuff

        /// <summary>
        /// Start Planting Mode.. Callback after any plant seed is selected from the UI scroll list
        /// </summary>
        /// <param name="itemID"></param>
        public void StartPlantingMode(int itemID)
        {
            ToggleTopInfoAndUpgradeButton(true);
            topInfoText.text = "Planting " + ItemDatabase.Instance.items[itemID].name + "\n Click on the tile to plant select seed";
            FieldManager.Instance.StartPlantingMode(itemID);
            //ToggleList(false);
        }

        /// <summary>
        /// Finish planting mode
        /// </summary>
        private void StopPlantingMode()
        {
            topInfoText.text = "";
            ToggleTopInfoAndUpgradeButton(false);
            FieldManager.Instance.StopPlantingMode();
        }

        #endregion

        #region Toggle different UI Menus parts

        private void ToggleTopInfoAndUpgradeButton(bool flag)
        {
            if (topInfoParentTransform != null)
            {
                topInfoParentTransform.gameObject.SetActive(flag);
            }
        }

        #endregion

    }
}