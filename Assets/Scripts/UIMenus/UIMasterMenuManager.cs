using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;
using System;
using UnityEngine.UI;

public class UIMasterMenuManager : MonoBehaviour
{
    public static UIMasterMenuManager Instance = null;

    [SerializeField]
    private SpriteAtlas itemAtlas;
    [SerializeField]
    private DraggableUIItem menuItemPrefab;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private GameObject itemScrollList;
    [SerializeField]
    private GameObject objectInfoPopup;

    public Action<int> DraggedItemEvent;

    private int unlockedItemCount = 0;
    private int selectedBuildingID = -1;
    private DraggableUIItem[] menuItems = new DraggableUIItem[12];
    private List<int> unlockedItemIDs = new List<int>();
    private bool isDropCompleted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpwanMenuItems();
        CheckForUnlockedItems();
        ToggleDisplayMenuUI(false);
    }

    private void SpwanMenuItems()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i] = (Instantiate(menuItemPrefab, parentTransform));
            menuItems[i].name = "UIItemList" + i;
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

    public void DisplayUIMasterMenu(int buildingID, int sourceID)
    {
        PopulateItemsInMasterMenu(buildingID, sourceID);
    }

    private void PopulateItemsInMasterMenu(int buildingID, int sourceID)
    {
        //print(buildingID);
        selectedBuildingID = buildingID;
        ToggleDisplayMenuUI(true);
        //MenuManager.Instance.DisableAllMenus();
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].gameObject.SetActive(false);
            menuItems[i].itemID = -1;
        }
        //unlockedItemCount = 0;

        for (int i = 0; i < unlockedItemIDs.Count; i++)
        {
            if (ItemDatabase.Instance.items[unlockedItemIDs[i]].sourceID == sourceID)
            {
                menuItems[i].gameObject.SetActive(true);
                menuItems[i].itemID = ItemDatabase.Instance.items[unlockedItemIDs[i]].itemID;
                //menuItems[unlockedItemCount].itemImage.sprite = itemAtlas.GetSprite(ItemDatabase.Instance.items[i].name);
                print(ItemDatabase.Instance.items[unlockedItemIDs[i]].coinCost.ToString());

                menuItems[i].itemNameText.text = ItemDatabase.Instance.items[unlockedItemIDs[i]].name;
                menuItems[i].itemCostText.text = ItemDatabase.Instance.items[unlockedItemIDs[i]].coinCost.ToString();
            }
        }

        //for (int i = 0; i < unlockedItemIDs.Count; i++)
        //{
        //    if (ItemDatabase.Instance.items[i] != null)
        //    {
        //        if (ItemDatabase.Instance.items[i].sourceID == SourceDatabase.Instance.sources[i].sourceID)
        //        {
        //            menuItems[i].itemID = ItemDatabase.Instance.items[i].itemID;
        //            //menuItems[unlockedItemCount].itemImage.sprite = itemAtlas.GetSprite(ItemDatabase.Instance.items[i].name);
        //            menuItems[i].itemAmountText.text = ItemDatabase.Instance.items[i].coinCost.ToString();
        //            menuItems[i].gameObject.SetActive(true);
        //            unlockedItemCount++;
        //        }
        //    }
        //}
        this.gameObject.SetActive(true);
    }

    public void OnDropComplete()
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
            ToggleDisplayMenuUI(false);
            isDropCompleted = false;
            print("OnDragComplete" + selectedBuildingID + " " + itemID);
        }
    }

    public void UpgradeBuildingPressed(int id)
    {
        MenuManager.Instance.BuildingUpgradeMenuSetActive(true);
    }

    public void ToggleDisplayMenuUI(bool flag)
    {
        if (flag)
        {
            itemScrollList.SetActive(true);
            objectInfoPopup.SetActive(true);
        } else
        {
            itemScrollList.SetActive(false);
            objectInfoPopup.SetActive(false);
        }
    }
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