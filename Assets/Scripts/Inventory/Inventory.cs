using HarvestValley.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory m_instance = null;
    public GameObject inventorySlotPanel, armourSlotPanel, chestSlotPanel;
    public GameObject inventorySlot, armorSlot, chestSlot;
    public GameObject inventoryItem;
    public int inventorySlotAmount = 10, armourSlotAmount = 4, chestSlotAmount = 6;
    public Image slotSelectedImage;
    public List<Item> items = new List<Item>();
    public List<GameObject> slotsGO = new List<GameObject>();
    public Item selectedItem = null;
    public int selectedSlotID = -1;
    public int maxStackAmount = 10;
    public Text debug;

    int inputFeildID = -1, inputFeildAmount = -1;
    InventoryItem[] myInventory;

    void Awake()
    {
        m_instance = this;
    }

    void Start()
    {
        for (int i = 0; i < inventorySlotAmount; i++)
        {
            items.Add(new Item());
            slotsGO.Add(Instantiate(inventorySlot, inventorySlotPanel.transform));
            slotsGO[i].GetComponent<InventorySlot>().id = i;
            slotsGO[i].GetComponent<RectTransform>().localScale = Vector3.one;
        }
        for (int i = inventorySlotAmount; i < inventorySlotAmount + armourSlotAmount; i++)
        {
            items.Add(new Item());
            slotsGO.Add(Instantiate(armorSlot, armourSlotPanel.transform));
            slotsGO[i].GetComponent<ArmourSlot>().id = i;
            slotsGO[i].GetComponent<RectTransform>().localScale = Vector3.one;
        }
        for (int i = inventorySlotAmount + armourSlotAmount; i < inventorySlotAmount + armourSlotAmount + chestSlotAmount; i++)
        {
            items.Add(new Item());
            slotsGO.Add(Instantiate(chestSlot, chestSlotPanel.transform));
            slotsGO[i].GetComponent<ChestSlot>().id = i;
            slotsGO[i].GetComponent<RectTransform>().localScale = Vector3.one;
        }

        myInventory = new InventoryItem[slotsGO.Count];

        AddItem(3);
        AddItem(3);
        AddItem(3);
        AddItem(5);
        AddItem(2);
        AddItem(2);
        AddItem(2);
        AddItem(2);
        AddItem(2);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(9);
        AddItem(7);
        Crafting.m_instance.CheckHighlight_ALL_CraftableItems();
    }

    public void AddItem(int id)
    {
        List<int> occurance = new List<int>();
        Item itemsToAdd = ItemDatabase.GetItemById(id);
        if (itemsToAdd.stackAmount > 0)
        {
            for (int i = 0; i < items.Count - (armourSlotAmount + chestSlotAmount); i++)
            {
                if (items[i].itemID == id)
                {
                    occurance.Add(i);
                }
            }
            if (occurance.Count > 0)
            {
                for (int i = 0; i < occurance.Count; i++)
                {
                    InventoryItemData data = slotsGO[occurance[i]].transform.GetChild(0).GetComponent<InventoryItemData>();
                    if (data.amount >= maxStackAmount)
                    {
                        if (i == occurance.Count - 1)
                        {
                            AddNewItemInUI(itemsToAdd);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        data.amount++;
                        data.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.amount.ToString();
                        break;
                    }
                }
            }
            else
            {
                AddNewItemInUI(itemsToAdd);
            }
        }
        else
        {
            AddNewItemInUI(itemsToAdd);
        }
        SaveInventoryItems();
        Crafting.m_instance.CheckHighlight_ALL_CraftableItems();
    }

    void AddNewItemInUI(Item itemsToAdd)
    {
        if (CheckInventoryHasAtleastOneSpace())
        {
            for (int i = 0; i < items.Count - (armourSlotAmount + chestSlotAmount); i++)
            {
                if (items[i].itemID == -1)
                {
                    items[i] = itemsToAdd;
                    GameObject itemsGO = Instantiate(inventoryItem, slotsGO[i].transform);
                    itemsGO.transform.SetAsFirstSibling();
                    itemsGO.GetComponent<RectTransform>().localScale = Vector3.one;
                    itemsGO.GetComponent<InventoryItemData>().slotID = i;
                    itemsGO.GetComponent<InventoryItemData>().type = itemsToAdd.type;
                    if (itemsToAdd.durability > 0)
                    {
                        itemsGO.GetComponent<InventoryItemData>().durability = itemsToAdd.durability;
                        itemsGO.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(90, 10);
                    }
                    else
                    {
                        itemsGO.GetComponent<InventoryItemData>().durability = -1;
                        itemsGO.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 10);
                    }
                    itemsGO.GetComponent<InventoryItemData>().item = itemsToAdd;
                    itemsGO.GetComponent<Image>().sprite = AtlasBank.Instance.GetSprite(items[i].slug, AtlasType.GUI);
                    itemsGO.GetComponent<RectTransform>().anchoredPosition = Vector3.one;
                    break;
                }
            }
        }
    }

    public void RemoveItem(int id)
    {
        Item itemsToRemove = ItemDatabase.GetItemById(id);
        //print ("removed " + itemsToRemove.Name);
        if (itemsToRemove.stackAmount > 0)
        {
            for (int i = 0; i < items.Count - (armourSlotAmount + chestSlotAmount); i++)
            {
                if (items[i].itemID == id)
                {
                    InventoryItemData data = slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>();
                    if (data.amount > 1)
                    {
                        data.amount--;
                        data.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.amount.ToString();
                    }
                    else
                    {
                        DestroyItem(i);
                    }
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemID == id)
                {
                    slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().amount--;
                    DestroyItem(i);
                    break;
                }
            }
        }
        //SaveInventoryItems ();
        Crafting.m_instance.CheckHighlight_ALL_CraftableItems();
    }

    public void DeleteSelectedItem()
    {
        /*if (selectedSlotID >= 0 && slotsGO [selectedSlotID].transform.childCount > 0 && slotsGO [selectedSlotID].transform.GetChild (0).CompareTag ("Item")) {
			l_items [selectedSlotID] = new MyItem ();
			Destroy (slotsGO [selectedSlotID].transform.GetChild (0).gameObject);
		}*/
        DestroyItem(selectedSlotID);
    }

    public void DestroyItem(int id)
    {
        for (int i = 0; i < slotsGO[id].transform.childCount; i++)
        {
            if (slotsGO[id].transform.GetChild(i).CompareTag("Item"))
            {
                DestroyImmediate(slotsGO[id].transform.GetChild(i).gameObject); // Be careful used DestroyImmediate, come here if there is any issue
                break;
            }
        }
        items[id] = new Item();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < slotsGO.Count; i++)
            {
                if (slotsGO[i].GetComponent<InventorySlot>())
                {
                    if (CheckIfSlotHasItem(i))
                    {
                        print("inv>>>>> " + slotsGO[i].GetComponent<InventorySlot>().id + " " + items[i].itemID + " " +
                        slotsGO[i].GetComponent<InventorySlot>().transform.GetChild(0).GetComponent<InventoryItemData>().amount);
                    }
                    else
                    {
                        print("inv>>>>> " + slotsGO[i].GetComponent<InventorySlot>().id + " " + items[i].itemID + " 0");
                    }
                }
                if (slotsGO[i].GetComponent<ArmourSlot>())
                {
                    if (CheckIfSlotHasItem(i))
                    {
                        print("arm  " + slotsGO[i].GetComponent<ArmourSlot>().id + " " + items[i].itemID + " " +
                        slotsGO[i].GetComponent<ArmourSlot>().transform.GetChild(0).GetComponent<InventoryItemData>().amount);
                    }
                    else
                    {
                        print("arm  " + slotsGO[i].GetComponent<ArmourSlot>().id + " " + items[i].itemID + " 0");
                    }
                }
                if (slotsGO[i].GetComponent<ChestSlot>())
                {
                    if (CheckIfSlotHasItem(i))
                    {
                        print("che0000000" + slotsGO[i].GetComponent<ChestSlot>().id + " " + items[i].itemID + " " +
                        slotsGO[i].GetComponent<ChestSlot>().transform.GetChild(0).GetComponent<InventoryItemData>().amount);
                    }
                    else
                    {
                        print("che0000000 " + slotsGO[i].GetComponent<ChestSlot>().id + " " + items[i].itemID + " 0");
                    }
                }
            }
        }
    }

    bool CheckItemInInventory(Item item)
    {
        for (int i = 0; i < inventorySlotAmount; i++)
        {
            if (items[i].itemID == item.itemID)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckIfSlotHasItem(int slotID)
    {
        if (slotsGO[slotID].transform.childCount > 0 && slotsGO[slotID].transform.GetChild(0).CompareTag("Item"))
        {
            return true;
        }
        return false;
    }

    public int CheckItemAmountInInventory(int id) //do this by slots or items saved
    {
        int amount = 0;
        for (int i = 0; i < inventorySlotAmount; i++)
        {
            if (slotsGO[i].transform.childCount > 0 && slotsGO[i].transform.GetChild(0).CompareTag("Item") && slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().item.itemID == id)
            {
                amount += slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().amount;
            }
        }
        return amount;
    }

    public bool CheckInventoryHasAtleastOneSpace()
    {
        for (int i = 0; i < inventorySlotAmount; i++)
        {
            if (slotsGO[i].transform.childCount <= 0)
            {
                return true;
            }
            else if (!slotsGO[i].transform.GetChild(0).CompareTag("Item"))
            {
                return true;
            }
        }
        print("unable to add inventory full");
        return false;
    }

    void SaveInventoryItems()
    {
        for (int i = 0; i < slotsGO.Count; i++)
        {
            if (slotsGO[i].transform.childCount > 0 && slotsGO[i].transform.GetChild(0).CompareTag("Item"))
            {
                myInventory[i] = new InventoryItem(slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().item.itemID,
                    slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().amount,
                    slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().slotID,
                    slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().item.durability);
            }
            else
            {
                myInventory[i] = new InventoryItem();
            }
        }
    }

    void LoadInventoryItems()
    {
        InventoryItem[] myInventory = new InventoryItem[slotsGO.Count];// = new InventoryItems ();
        for (int i = 0; i < slotsGO.Count; i++)
        {
            if (slotsGO[i].transform.childCount > 0 && slotsGO[i].transform.GetChild(0).CompareTag("Item"))
            {
                myInventory[i].ID = slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().item.itemID;
                myInventory[i].Amount = slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().amount;
                myInventory[i].SlotID = slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().slotID;
                myInventory[i].Health = slotsGO[i].transform.GetChild(0).GetComponent<InventoryItemData>().item.durability;
            }
            else
            {
                myInventory[i] = new InventoryItem();
            }
        }
    }

    public void DecreseWeaponDurability(int amount)
    {
        if (selectedSlotID >= 0 && slotsGO[selectedSlotID].transform.childCount > 0 &&
            slotsGO[selectedSlotID].transform.GetChild(0).CompareTag("Item") &&
            slotsGO[selectedSlotID].transform.GetChild(0).GetComponent<InventoryItemData>().durability >= 0)
        {
            slotsGO[selectedSlotID].transform.GetChild(0).GetComponent<InventoryItemData>().DecreaseItemDurability(amount);
        }
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

    public InventoryItem(int id, int amount, int slotID, int health)
    {
        this.ID = id;
        this.Amount = amount;
        this.SlotID = slotID;
        this.Health = health;
    }

    public InventoryItem()
    {
        this.ID = -1;
        this.Amount = -1;
        this.SlotID = -1;
        this.Health = -1;
    }
}