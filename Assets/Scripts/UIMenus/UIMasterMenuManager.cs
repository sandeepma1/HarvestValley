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
    private SpriteAtlas itemAtlas;
    [SerializeField]
    private ClickableUIItems scrollListItemPrefab;
    [SerializeField]
    private Transform scrollListParentTransform;
    [SerializeField]
    private GameObject uiItemScrollList;
    [SerializeField]
    private GameObject uiObjectInfoMenu;
    [SerializeField]
    private GameObject uiHarvestMenu;
    [SerializeField]
    private GameObject navigationButtonsGroup;

    public Action<int> DraggedItemEvent;

    private int selectedBuildingID = -1;
    private int selectedSourceID = -1;
    private ClickableUIItems[] menuItems = new ClickableUIItems[12];
    private List<int> unlockedItemIDs = new List<int>();
    private bool isDropCompleted = false;

    private void Start()
    {
        SpwanMenuItems();
        CheckForUnlockedItems();
        ToggleScrolItemlList(false);
        ToggleHarvestMenu(false);
        ToggleObjectInfoMenu(false);
    }

    private void SpwanMenuItems()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i] = (Instantiate(scrollListItemPrefab, scrollListParentTransform));
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
        //foreach (var item in unlockedItemIDs)
        //{
        //    print("unlocked items " + item);
        //}
    }

    public void DisplayUIMasterMenuToPlantSeed(int buildingID, int sourceID)
    {
        selectedBuildingID = buildingID;
        selectedSourceID = sourceID;

        //ToggleObjectInfoMenu(true);
        ToggleScrolItemlList(true);
        PopulateItemsInMasterMenu(buildingID, sourceID);
    }

    public void DisplayUIMasterMenuToHarvest(int buildingID, int sourceID)
    {
        selectedBuildingID = buildingID;
        selectedSourceID = sourceID;

        ToggleObjectInfoMenu(true);
        ToggleHarvestMenu(true); //TODO: send selected crop images
    }

    private void PopulateItemsInMasterMenu(int buildingID, int sourceID)
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
                menuItems[i].gameObject.SetActive(true);
                menuItems[i].itemID = ItemDatabase.Instance.items[unlockedItemIDs[i]].itemID;
                //menuItems[unlockedItemCount].itemImage.sprite = itemAtlas.GetSprite(ItemDatabase.Instance.items[i].name);

                menuItems[i].itemNameText.text = ItemDatabase.Instance.items[unlockedItemIDs[i]].name;
                menuItems[i].itemCostText.text = ItemDatabase.Instance.items[unlockedItemIDs[i]].coinCost.ToString();
            }
            this.gameObject.SetActive(true);
        }
    }

    public void OnHarvestComplete()
    {
        BuildingsManager.Instance.HarvestCropOnFarmLand(selectedBuildingID);
        ToggleHarvestMenu(false);
        ToggleScrolItemlList(true);
        PopulateItemsInMasterMenu(selectedBuildingID, selectedSourceID);
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
            BuildingsManager.Instance.PlantItemOnBuilding(selectedBuildingID, itemID);
            selectedBuildingID = -1;
            ToggleScrolItemlList(false);
            ToggleObjectInfoMenu(false);
            isDropCompleted = false;
        }
    }

    public void OnUIItemClicked(int itemID)
    {
        if (selectedBuildingID == -1)
        {
            return;
        }
        BuildingsManager.Instance.PlantItemOnBuilding(selectedBuildingID, itemID);
        selectedBuildingID = -1;
    }

    public void UpgradeBuildingPressed(int id)
    {
        MenuManager.Instance.BuildingUpgradeMenuSetActive(true);
    }

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

    private void ToggleHarvestMenu(bool flag)
    {
        uiHarvestMenu.SetActive(flag);
    }

    #region UIbuttons functions
    public void CloseUIMasterMenu()
    {
        ToggleScrolItemlList(false);
        ToggleObjectInfoMenu(false);
        ToggleHarvestMenu(false);
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