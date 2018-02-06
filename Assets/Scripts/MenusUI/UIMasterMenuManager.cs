using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;
using System;
using UnityEngine.UI;

public class UIMasterMenuManager : Singleton<UIMasterMenuManager>
{
    public Canvas mainCanvas;
    [SerializeField]
    private ClickableUIItems scrollListItemPrefab;
    [SerializeField]
    private Transform scrollList;
    [SerializeField]
    private GameObject uiObjectInfoMenu;
    [SerializeField]
    private GameObject navigationButtonsGroup;
    [SerializeField]
    private GameObject uiItemScrollList;
    [SerializeField]
    private Button clickToFinishPlantingModeButton;

    public Action<int> DraggedItemEvent;

    private int selectedBuildingID = -1;
    private int selectedSourceID = -1;
    private ClickableUIItems[] menuItems = new ClickableUIItems[12];
    private List<int> unlockedItemIDs = new List<int>();
    private bool isDropCompleted = false;

    private void Start()
    {
        clickToFinishPlantingModeButton.onClick.AddListener(OnFinishButtonClicked);
        SpwanMenuItems();
        CheckForUnlockedItems();
        ToggleScrolItemlList(false);
        ToggleObjectInfoMenu(false);
        ToggleList(false);
        ToggleFinishButton(false);
    }

    private void SpwanMenuItems()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i] = (Instantiate(scrollListItemPrefab, scrollList));
            menuItems[i].name = "UIItemListClick" + i;
        }
    }

    public void CheckForUnlockedItems()  // call on level change & game start only
    {
        unlockedItemIDs.Clear();
        for (int i = 0; i <= PlayerProfileManager.Instance.CurrentPlayerLevel(); i++)
        {
            if (LevelUpDatabase.Instance.gameLevels[i].itemUnlockID >= 0)
            {
                unlockedItemIDs.Add(LevelUpDatabase.Instance.gameLevels[i].itemUnlockID);
            }
        }
    }

    #region Planting Mode Stuff

    /// <summary>
    /// Will Display Plant seed list as UI scroll list
    /// </summary>
    /// <param name="buildingID"></param>
    /// <param name="sourceID"></param>
    public void DisplayPlantSeedList(int buildingID, int sourceID)
    {
        selectedBuildingID = buildingID;
        selectedSourceID = sourceID;
        ToggleScrolItemlList(true);
        ToggleList(true);
        PopulateItemList(buildingID, sourceID);
    }

    /// <summary>
    /// Start Planting Mode.. Callback after any plant seed is selected from the UI scroll list
    /// </summary>
    /// <param name="itemID"></param>
    public void OnUIItemClicked(int itemID)
    {
        BuildingsManager.Instance.StartPlantingMode(itemID);
        //ToggleList(false);
        ToggleFinishButton(true);
    }

    /// <summary>
    /// Finish planting mode
    /// </summary>
    private void OnFinishButtonClicked()
    {
        BuildingsManager.Instance.StopPlantingMode();
        ToggleFinishButton(false);
        ToggleScrolItemlList(false);
    }

    #endregion

    private void PopulateItemList(int buildingID, int sourceID)
    {
        selectedBuildingID = buildingID;
        selectedSourceID = sourceID;
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].gameObject.SetActive(false);
            menuItems[i].itemID = -1;
        }

        for (int i = 0; i < unlockedItemIDs.Count; i++)
        {
            if (ItemDatabase.Instance.items[unlockedItemIDs[i]].sourceID == sourceID)
            {
                Item item = ItemDatabase.Instance.items[unlockedItemIDs[i]];

                menuItems[i].gameObject.SetActive(true);
                menuItems[i].itemID = item.itemID;
                menuItems[i].itemNameText.text = item.name;
                menuItems[i].itemCostText.text = item.coinCost.ToString();
                menuItems[i].itemImage.sprite = AtlasBank.Instance.GetSprite(item.slug, AtlasType.GUI);
            }
            this.gameObject.SetActive(true);
        }
    }

    public void OnItemDropComplete()
    {
        isDropCompleted = true;
    }

    public void OnDragComplete(int itemID)
    {
        if (isDropCompleted)
        {
            if (selectedBuildingID == -1)
            {
                return;
            }
            //BuildingsManager.Instance.PlantItemOnBuilding(selectedBuildingID, itemID);
            selectedBuildingID = -1;
            ToggleScrolItemlList(false);
            ToggleObjectInfoMenu(false);
            isDropCompleted = false;
        }
    }

    public void UpgradeBuildingPressed(int id)
    {
        MenuManager.Instance.BuildingUpgradeMenuSetActive(true);
    }


    #region Toggle different UI Menus, Lsists, etc

    private void ToggleObjectInfoMenu(bool flag)
    {
        uiObjectInfoMenu.SetActive(flag);
        navigationButtonsGroup.SetActive(!flag);
        if (!flag)
        {
            selectedBuildingID = -1;
            selectedSourceID = -1;
        }
    }

    private void ToggleScrolItemlList(bool flag)
    {
        uiItemScrollList.SetActive(flag);
    }

    private void ToggleList(bool flag)
    {
        scrollList.gameObject.SetActive(flag);
    }

    private void ToggleFinishButton(bool flag)
    {
        clickToFinishPlantingModeButton.gameObject.SetActive(flag);
    }

    #endregion


    #region UIbuttons functions
    public void CloseUIMasterMenu()
    {
        ToggleScrolItemlList(false);
        ToggleObjectInfoMenu(false);
        ToggleList(false);
        ToggleFinishButton(false);
        OnFinishButtonClicked();
    }
    #endregion
}

//public void ChildCallingOnMouseUp(int id)
//{
//    isItemSelected = false;
//    BuildingsManager.Instance.plantedOnSelectedfield = false;
//    itemSelectedID = -1;
//    ItemPopupProduction.Instance.HideItemPopupProduction();
//}

//public void ChildCallingOnMouseDown(int id, Vector2 pos)
//{
//    isItemSelected = true;
//    itemSelectedID = id;
//    if (BuildingsManager.Instance.BuildingsGO[BuildingsManager.Instance.buildingSelectedID].GetComponent<DraggableBuildings>().buildingID > 0)
//    {
//        ItemPopupProduction.Instance.DisplayItemPopupProduction_DOWN(id, pos);
//    }
//}

//public void ChildCallingOnMouseDrag(int id, Vector2 pos)
//{
//    isItemSelected = true;
//    itemSelectedID = id;
//    ToggleDisplayCropMenu();
//    if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].source != ItemSource.Field)
//    {
//        ItemPopupProduction.Instance.DisplayItemPopupProduction_DRAG(pos);
//    }
//}