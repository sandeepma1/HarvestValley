﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;

public class MasterMenuManager : MonoBehaviour
{
    public static MasterMenuManager Instance = null;
    public bool isItemSelected = false;
    public int itemSelectedID = -1;

    [SerializeField]
    private SpriteAtlas itemAtlas;
    [SerializeField]
    private GameObject switchButton;
    [SerializeField]
    private GameObject fieldInfoButton;
    [SerializeField]
    private DraggableItems menuItemPrefab;

    private Vector2[] itemPos = new Vector2[4];
    private int pageNumber = 0;
    private int maxItemsOnceInMenu = 4;
    private int maxPages = 0;
    private int unlockedItemCount = 0;
    private DraggableItems[] menuItems = new DraggableItems[12];
    private List<int> unlockedItemIDs = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        itemPos[0] = new Vector2(-0.5f, -0.6f);
        itemPos[1] = new Vector2(0.5f, -0.6f);
        itemPos[2] = new Vector2(-0.5f, 0.25f);
        itemPos[3] = new Vector2(0.5f, 0.25f);
        SpwanMenuItems();
        CheckForUnlockedItems();
    }

    private void SpwanMenuItems()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i] = (Instantiate(menuItemPrefab, this.gameObject.transform.GetChild(0).transform));
            menuItems[i].name = "Item" + i;
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

        if (unlockedItemIDs.Count > 4)
        {
            switchButton.SetActive(true);
        }
        maxPages = unlockedItemIDs.Count / maxItemsOnceInMenu;
        if (unlockedItemIDs.Count % maxItemsOnceInMenu >= 1)
        {
            maxPages++;
        }
        foreach (var item in unlockedItemIDs)
        {
            //print("unlocked items " + item);
        }
    }

    public void PopulateItemsInMasterMenu(int buildingID)
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].gameObject.SetActive(false);
            menuItems[i].itemID = -1;
        }
        int posCount = 0;
        unlockedItemCount = 0;
        pageNumber = 0;
        //isMasterMenuUp = true;
        for (int i = 0; i < unlockedItemIDs.Count; i++)
        {
            if (ItemDatabase.Instance.items[i] != null && ItemDatabase.Instance.items[i].source == BuildingDatabase.Instance.buildingInfo[buildingID].name)
            {
                menuItems[unlockedItemCount].itemID = ItemDatabase.Instance.items[i].id;
                menuItems[unlockedItemCount].transform.localPosition = itemPos[posCount];
                menuItems[unlockedItemCount].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = itemAtlas.GetSprite(ItemDatabase.Instance.items[i].name);
                menuItems[posCount].transform.GetChild(1).GetComponent<TextMeshPro>().text = PlayerInventoryManager.Instance.playerInventory[i].count.ToString();
                posCount++;
                unlockedItemCount++;
                if (posCount > itemPos.Length - 1)
                {
                    posCount = 0;
                }
            }
        }
        maxPages = unlockedItemCount / maxItemsOnceInMenu;
        if (unlockedItemCount % maxItemsOnceInMenu >= 1)
        { //Calculate Max pages
            maxPages++;
        }
        ToggleMenuPages();
    }

    public void UpdateSeedValue()
    {
        for (int i = 0; i < unlockedItemCount; i++)
        {
            menuItems[i].transform.GetChild(1).GetComponent<TextMeshPro>().text = PlayerInventoryManager.Instance.playerInventory[i].count.ToString();
        }
        PlayerInventoryManager.Instance.UpdateScrollListItemCount();
    }

    public void ToggleMenuPages()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].gameObject.SetActive(false);
        }
        int loopCount = 0;
        for (int i = pageNumber * maxItemsOnceInMenu; i < unlockedItemCount; i++)
        {
            menuItems[i].gameObject.SetActive(true);
            loopCount++;
            if (loopCount >= 4)
            {
                break;
            }
        }
        pageNumber++;
        if (pageNumber >= maxPages)
        {
            pageNumber = 0;
        }
    }

    public void UpgradeBuildingPressed(int id)
    {
        MenuManager.Instance.BuildingUpgradeMenuSetActive(true);
    }

    public void ChildCallingOnMouseUp(int id)
    {
        isItemSelected = false;
        BuildingsManager.Instance.plantedOnSelectedfield = false;
        itemSelectedID = -1;
        ItemPopupProduction.Instance.HideItemPopupProduction();
    }

    public void ChildCallingOnMouseDown(int id, Vector2 pos)
    {
        isItemSelected = true;
        itemSelectedID = id;
        if (BuildingsManager.Instance.BuildingsGO[BuildingsManager.Instance.buildingSelectedID].GetComponent<DraggableBuildings>().buildingID > 0)
        {
            ItemPopupProduction.Instance.DisplayItemPopupProduction_DOWN(id, pos);
        }
    }

    public void ChildCallingOnMouseDrag(int id, Vector2 pos)
    {
        isItemSelected = true;
        itemSelectedID = id;
        ToggleDisplayCropMenu();
        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].source != ItemSource.Field)
        {
            ItemPopupProduction.Instance.DisplayItemPopupProduction_DRAG(pos);
        }

    }

    public void ToggleDisplayCropMenu()
    {
        transform.position = new Vector3(-500, -500, 0);
    }
}