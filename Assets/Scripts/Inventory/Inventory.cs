using HarvestValley.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory>
{
    public Transform inventorySlotPanel, armourSlotPanel, backpackSlotPanel;
    [SerializeField]
    private InventorySlot inventorySlotPrefab;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemDescription;
    public int inventorySlotAmount = 10, armourSlotAmount = 4, chestSlotAmount = 6;
    public Transform slotSelector;
    public List<InventorySlot> InventorySlotsGO = new List<InventorySlot>();

    private int inputFeildID = -1, inputFeildAmount = -1;
    private InventoryItem[] myInventory;

    private void Start()
    {
        for (int i = 0; i < inventorySlotAmount; i++)
        {
            SpawnInventorySlot(i, SlotType.Inventory, inventorySlotPanel);
        }
        for (int i = inventorySlotAmount; i < inventorySlotAmount + armourSlotAmount; i++)
        {
            SpawnInventorySlot(i, SlotType.Armor, armourSlotPanel);
        }
        for (int i = inventorySlotAmount + armourSlotAmount; i < inventorySlotAmount + armourSlotAmount + chestSlotAmount; i++)
        {
            SpawnInventorySlot(i, SlotType.Backpack, backpackSlotPanel);
        }

        myInventory = new InventoryItem[InventorySlotsGO.Count];
        AddItem(22);
        //AddItem(3); AddItem(3); AddItem(5); AddItem(2); AddItem(2); AddItem(2);
        //AddItem(2); AddItem(2); AddItem(1); AddItem(1); AddItem(1); AddItem(9); AddItem(7);
        //Crafting.m_instance.CheckHighlight_ALL_CraftableItems();
    }

    private void SpawnInventorySlot(int id, SlotType slotType, Transform panelTransform)
    {
        InventorySlotsGO.Add(Instantiate(inventorySlotPrefab, panelTransform));
        InventorySlotsGO[id].slotId = id;
        InventorySlotsGO[id].slotType = slotType;
        InventorySlotsGO[id].ItemId = -1;
        InventorySlotsGO[id].OnSlotClicked += OnSlotClickedEventhandler;
        //InventorySlotsGO[id].GetComponent<RectTransform>().localScale = Vector3.one;
    }

    private void OnSlotClickedEventhandler(int slotId)
    {
        slotSelector.position = InventorySlotsGO[slotId].transform.position;
        ShowSelectedSlotDetails(slotId);
    }

    private void ShowSelectedSlotDetails(int slotId)
    {
        if (InventorySlotsGO[slotId].ItemId > 0)
        {
            Item item = ItemDatabase.GetItemById(InventorySlotsGO[slotId].ItemId);
            itemName.text = item.name;
            itemDescription.text = item.description;
        }
        else
        {
            itemName.text = "";
            itemDescription.text = "";
        }

    }

    public void AddItem(int itemId)
    {
        List<int> occurance = new List<int>();
        int itemStackAmount = ItemDatabase.GetItemStackAmountById(itemId);

        if (itemStackAmount > 0)
        {
            for (int i = 0; i < InventorySlotsGO.Count - (armourSlotAmount + chestSlotAmount); i++) // adding only in inventory!!
            {
                if (InventorySlotsGO[i].ItemId == itemId)
                {
                    occurance.Add(i);
                }
            }
            if (occurance.Count > 0)
            {
                for (int i = 0; i < occurance.Count; i++)
                {
                    if (InventorySlotsGO[occurance[i]].Amount >= itemStackAmount)
                    {
                        if (i == occurance.Count - 1)
                        {
                            AddNewItemInUI(itemId);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        InventorySlotsGO[occurance[i]].Amount++;
                        break;
                    }
                }
            }
            else
            {
                AddNewItemInUI(itemId);
            }
        }
        else
        {
            AddNewItemInUI(itemId);
        }
        //SaveInventoryItems();
        //Crafting.m_instance.CheckHighlight_ALL_CraftableItems();
    }

    private void AddNewItemInUI(int itemId)
    {
        int emptySlotId = GetEmptySlotId();
        if (emptySlotId == -1)
        {
            print("All slots full");
            return;
        }
        else
        {
            InventorySlotsGO[emptySlotId].ItemId = itemId;
        }
    }

    public void RemoveItem(int itemId)
    {
        int itemStackAmount = ItemDatabase.GetItemStackAmountById(itemId);
        if (itemStackAmount > 0)
        {
            for (int i = 0; i < InventorySlotsGO.Count - (armourSlotAmount + chestSlotAmount); i++)
            {
                if (InventorySlotsGO[i].ItemId == itemId)
                {
                    if (InventorySlotsGO[i].Amount > 1)
                    {
                        InventorySlotsGO[i].Amount--;
                    }
                    else
                    {
                        InventorySlotsGO[i].EmptySlot();
                    }
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < InventorySlotsGO.Count - (armourSlotAmount + chestSlotAmount); i++)
            {
                if (InventorySlotsGO[i].ItemId == itemId)
                {
                    InventorySlotsGO[i].Amount--;
                    break;
                }
            }
        }
        //SaveInventoryItems ();
        //Crafting.m_instance.CheckHighlight_ALL_CraftableItems();
    }

    public int CheckItemAmountInInventory(int id) //do this by slots or items saved
    {
        int amount = 0;
        for (int i = 0; i < inventorySlotAmount; i++)
        {
            if (InventorySlotsGO[i].ItemId == id)
            {
                amount += InventorySlotsGO[i].Amount;
            }
        }
        return amount;
    }

    public int GetEmptySlotId()
    {
        for (int i = 0; i < InventorySlotsGO.Count - (armourSlotAmount + chestSlotAmount); i++) // adding only in inventory!!
        {
            if (InventorySlotsGO[i].ItemId == -1)
            {
                return i;
            }
        }
        print("unable to add inventory full");
        return -1;
    }

    public void AddItemButton()
    {
        AddItem(inputFeildID);
    }

    public void RemoveItemButton()
    {
        RemoveItem(inputFeildID);
    }

    public void InputFeildID(string text)
    {
        int id;
        int.TryParse(text, out id);
        inputFeildID = id;
    }

    public void InputFeildAmount(string text)
    {
        int amount;
        int.TryParse(text, out amount);
        inputFeildAmount = amount;
    }
}

[System.Serializable]
public class InventoryItem
{
    public int ID { get; set; }

    public int Amount { get; set; }

    public int SlotID { get; set; }

    public int Health { get; set; }

    public SlotType SlotType { get; set; }

    public InventoryItem(int id, int amount, int slotID, int health, SlotType slotType)
    {
        ID = id;
        Amount = amount;
        SlotID = slotID;
        Health = health;
        SlotType = slotType;
    }

    public InventoryItem()
    {
        ID = -1;
        Amount = -1;
        SlotID = -1;
        Health = -1;
    }
}

public enum SlotType
{
    Backpack, Inventory, Armor, Crafting
}