using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiInventoryMenu : Singleton<UiInventoryMenu>
{
    public GameObject ListPrefab, ScrollListGO;
    public List<FarmItems> playerInventory = new List<FarmItems>();
    //public FarmItems[] playerInventory;

    List<GameObject> listItems = new List<GameObject>();

    void Start()
    {
        playerInventory = ES2.LoadList<FarmItems>("PlayerInventory");
        PopulateScrollListAtStart();
        InvokeRepeating("SavePlayerInventory", 3, 3);
    }

    public void PopulateScrollListAtStart()
    {
        for (int i = 0; i < playerInventory.Count; i++)
        {
            AddOneItemInScrollList(i);
        }
    }

    void AddOneItemInScrollList(int scrollListID)
    {
        listItems.Add(Instantiate(ListPrefab, ScrollListGO.transform));
        listItems[scrollListID].GetComponent<RectTransform>().localScale = Vector3.one; //fixed some scaling bug
        listItems[scrollListID].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInventory[scrollListID].count.ToString();
        listItems[scrollListID].GetComponent<Image>().sprite = AtlasBank.Instance.GetSprite(ItemDatabase.Instance.items[scrollListID].slug, AtlasType.GUI);
        listItems[scrollListID].name = "InventoryListItem" + scrollListID;
        //print(listItems[scrollListID].name);
    }

    public void UpdateScrollListItemCount()
    {
        for (int i = 0; i < playerInventory.Count; i++)
        {
            listItems[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInventory[i].count.ToString();
        }
    }

    public void UpdateFarmItems(int id, int value)
    {
        foreach (var item in playerInventory)
        {
            if (item.id == id)
            {
                item.count += value;
            }
        }
        UpdateScrollListItemCount();
    }

    public void AddNewFarmItem(int id, int value)
    {
        playerInventory.Add(new FarmItems(id, value));
        AddOneItemInScrollList(playerInventory.Count - 1);
        UpdateScrollListItemCount();
    }

    void SavePlayerInventory()
    {
        ES2.Save(playerInventory, "PlayerInventory");
    }
}

public class FarmItems
{
    public int id;
    public int count;

    public FarmItems(int i_id, int i_count)
    {
        id = i_id;
        count = i_count;
    }

    public FarmItems()
    {

    }
}