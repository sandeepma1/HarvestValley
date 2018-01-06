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
    public bool isItemSelected = false;
    private int itemSelectedID = -1;

    [SerializeField]
    private SpriteAtlas itemAtlas;
    [SerializeField]
    private DraggableUIItem menuItemPrefab;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private GameObject scrollList;

    public Action<int> ItemSelectedEvent;

    private int unlockedItemCount = 0;
    private DraggableUIItem[] menuItems = new DraggableUIItem[12];
    private List<int> unlockedItemIDs = new List<int>();

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

    private void SelectedItemID(int id)
    {
        itemSelectedID = id;
        BuildingsManager.Instance.itemSelectedID = id;
    }

    private void SpwanMenuItems()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i] = (Instantiate(menuItemPrefab, parentTransform));
            menuItems[i].name = "UIItemList" + i;
            menuItems[i].SelectedItemID += SelectedItemID;
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
        foreach (var item in unlockedItemIDs)
        {
            print("unlocked items " + item);
        }
    }

    public void PopulateItemsInMasterMenu(int buildingID)
    {
        ToggleDisplayMenuUI(true);
        //MenuManager.Instance.DisableAllMenus();
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].gameObject.SetActive(false);
            menuItems[i].itemID = -1;
        }
        unlockedItemCount = 0;

        for (int i = 0; i < unlockedItemIDs.Count; i++)
        {
            if (ItemDatabase.Instance.items[i] != null && ItemDatabase.Instance.items[i].source == BuildingDatabase.Instance.buildingInfo[buildingID].name)
            {
                menuItems[unlockedItemCount].itemID = ItemDatabase.Instance.items[i].id;
                //menuItems[unlockedItemCount].itemImage.sprite = itemAtlas.GetSprite(ItemDatabase.Instance.items[i].name);
                menuItems[unlockedItemCount].itemAmountText.text = ItemDatabase.Instance.items[i].coinCost.ToString();
                menuItems[i].gameObject.SetActive(true);
                unlockedItemCount++;
            }
        }
        this.gameObject.SetActive(true);
    }

    public void UpdateSeedValue()
    {
        for (int i = 0; i < unlockedItemCount; i++)
        {
            //menuItems[i].transform.GetChild(1).GetComponent<TextMeshPro>().text = PlayerInventoryManager.Instance.playerInventory[i].count.ToString();
        }
        PlayerInventoryManager.Instance.UpdateScrollListItemCount();
    }

    public void UpgradeBuildingPressed(int id)
    {
        MenuManager.Instance.BuildingUpgradeMenuSetActive(true);
    }

    public void ToggleDisplayMenuUI(bool flag)
    {
        if (flag)
        {
            scrollList.SetActive(true);
        } else
        {
            scrollList.SetActive(false);
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