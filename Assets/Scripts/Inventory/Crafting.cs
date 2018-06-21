using HarvestValley.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public static Crafting m_instance = null;
    public GameObject craftPanel;
    public GameObject craftSlot;
    public Image slotSelectedImage;
    public int[] craftingItems;
    public List<GameObject> craftingSlotsGO = new List<GameObject>();

    public int selectedItemID = -1;

    void Awake()
    {
        m_instance = this;
    }

    void Start()
    {
        //CraftingSlot craftingSlot = new CraftingSlot();
        for (int i = 0; i < craftingItems.Length; i++)
        {
            craftingSlotsGO.Add(Instantiate(craftSlot, craftPanel.transform));
            craftingSlotsGO[i].GetComponent<CraftingSlot>().id = i;
            craftingSlotsGO[i].GetComponent<CraftingSlot>().itemID = craftingItems[i];
            craftingSlotsGO[i].GetComponent<RectTransform>().localScale = Vector3.one;
            craftingSlotsGO[i].transform.GetChild(0).GetComponent<Image>().sprite = AtlasBank.Instance.GetSprite(
                ItemDatabase.GetItemById(craftingItems[i]).slug, AtlasType.GUI);
        }
        //CheckHighlight_ALL_CraftableItems();
    }

    public void CheckHighlight_ALL_CraftableItems()
    {
        //	print ("checking");
        for (int i = 0; i < craftingItems.Length; i++)
        {
            if (CheckForRequiredItemsInInventory(craftingItems[i]))
            {
                craftingSlotsGO[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                craftingSlotsGO[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
            }
        }
    }

    public void CraftSelectedItem()
    {
        if (selectedItemID >= 0)
        {
            if (CheckForRequiredItemsInInventory(selectedItemID) && Inventory.Instance.GetEmptySlotId() != -1)
            { //if inventory has all the craftable items
                RemoveItemsToCreateNewItem();
                Inventory.Instance.AddItem(selectedItemID);
                print("crafted item" + selectedItemID);
            }
            else
            {
                print("missing Items or inventory full");
            }
        }
    }

    void RemoveItemsToCreateNewItem()
    {
        Item itemToCraft = ItemDatabase.GetItemById(selectedItemID);

        if (itemToCraft.needID[0] >= 0)
        {
            for (int i = 0; i < itemToCraft.needAmount[0]; i++)
            {
                Inventory.Instance.RemoveItem(itemToCraft.needID[0]);
            }
        }
        if (itemToCraft.needID[1] >= 0)
        {
            for (int i = 0; i < itemToCraft.needAmount[1]; i++)
            {
                Inventory.Instance.RemoveItem(itemToCraft.needID[1]);
            }
        }
        if (itemToCraft.needID[2] >= 0)
        {
            for (int i = 0; i < itemToCraft.needAmount[2]; i++)
            {
                Inventory.Instance.RemoveItem(itemToCraft.needID[2]);
            }
        }
        if (itemToCraft.needID[3] >= 0)
        {
            for (int i = 0; i < itemToCraft.needAmount[3]; i++)
            {
                Inventory.Instance.RemoveItem(itemToCraft.needID[3]);
            }
        }
    }

    bool CheckForRequiredItemsInInventory(int id)
    {
        Item itemToCraft = ItemDatabase.GetItemById(id);
        int maxItems = itemToCraft.needAmount.Length;
        bool[] needItem = new bool[maxItems];

        for (int i = 0; i < maxItems; i++)
        {
            if (itemToCraft.needID[i] >= -1 && Inventory.Instance.CheckItemAmountInInventory(itemToCraft.needID[i]) >= itemToCraft.needAmount[i])
            {
                needItem[i] = true;
            }
        }

        for (int i = 0; i < maxItems; i++)
        {
            if (needItem[i] == false)
            {
                return false;
            }
        }
        return true;

    }
}
