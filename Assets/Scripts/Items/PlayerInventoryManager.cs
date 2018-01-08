using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    public GameObject ListPrefab, ScrollListGO;
    public static PlayerInventoryManager Instance = null;
    public List<FarmItems> playerInventory = new List<FarmItems>();
    //public FarmItems[] playerInventory;

    List<GameObject> listItems = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        NewGameStart();
        //playerItems = ES2.LoadList<FarmItems> ("playerInventory");
        playerInventory = ES2.LoadList<FarmItems>("playerInventory");
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
        listItems[scrollListID].GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Textures/Items/" + ItemDatabase.Instance.items[scrollListID].name);
        listItems[scrollListID].name = "InventoryListItem" + scrollListID;
        print(listItems[scrollListID].name);
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
        ES2.Save(playerInventory, "playerInventory");
    }

    #region Init FarmStorage

    void NewGameStart()
    {
        if (PlayerPrefs.GetInt("playerInventory") <= 0)
        {
            ES2.Delete("playerInventory");
            playerInventory.Add(new FarmItems(0, 9)); //addding Wheat for the first level
                                                      //playerInventory.Add (new FarmItems (1, 3));
            ES2.Save(playerInventory, "playerInventory");
            PlayerPrefs.SetInt("playerInventory", 1);
        }
    }

    #endregion
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