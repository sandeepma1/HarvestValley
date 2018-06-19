using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerDownHandler
{
    public int id;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemData droppedItem = eventData.pointerDrag.GetComponent<InventoryItemData>();

        if (Inventory.m_instance.items[id].itemID == -1)
        {
            Inventory.m_instance.items[droppedItem.slotID] = new Item();
            Inventory.m_instance.items[id] = droppedItem.item;
            droppedItem.slotID = id;
        }
        else
        {
            if (droppedItem.slotID >= Inventory.m_instance.inventorySlotAmount && droppedItem.slotID < Inventory.m_instance.armourSlotAmount + Inventory.m_instance.inventorySlotAmount)
            {
                return;
            }
            Transform item = null;
            foreach (Transform transforms in this.transform)
            {
                if (transforms.CompareTag("Item"))
                {
                    item = transforms;
                }
            }
            if (item != null)
            {
                if (item.GetComponent<InventoryItemData>().item.itemID == droppedItem.item.itemID && droppedItem.item.stackAmount > 0)
                {   //if item is dropped on	same item
                    if (item.GetComponent<InventoryItemData>().amount + droppedItem.amount > Inventory.m_instance.maxStackAmount)
                    { //if both item sum is grater tahn max stack amount
                        droppedItem.amount = (item.GetComponent<InventoryItemData>().amount + droppedItem.amount) - Inventory.m_instance.maxStackAmount;
                        droppedItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = droppedItem.amount.ToString();
                        item.GetComponent<InventoryItemData>().amount = Inventory.m_instance.maxStackAmount;
                        item.GetComponent<InventoryItemData>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<InventoryItemData>().amount.ToString();
                    }
                    else
                    {
                        item.GetComponent<InventoryItemData>().amount += droppedItem.amount;
                        item.GetComponent<InventoryItemData>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<InventoryItemData>().amount.ToString();
                        Inventory.m_instance.items[droppedItem.slotID] = new Item();
                        DestroyImmediate(droppedItem.gameObject);
                        print("added and deleted down item " + droppedItem.GetComponent<InventoryItemData>().item.name);
                    }
                }
                else
                {//swap if not same item
                    item.GetComponent<InventoryItemData>().slotID = droppedItem.slotID;
                    item.transform.SetParent(Inventory.m_instance.slotsGO[droppedItem.slotID].transform);
                    item.transform.position = Inventory.m_instance.slotsGO[droppedItem.slotID].transform.position;

                    Inventory.m_instance.items[droppedItem.slotID] = item.GetComponent<InventoryItemData>().item;
                    Inventory.m_instance.items[id] = droppedItem.item;

                    droppedItem.slotID = id;
                    droppedItem.transform.SetParent(this.transform);
                    droppedItem.transform.position = this.transform.position;
                }
            }
        }
        SelectSlot();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSlot();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectSlot();
    }

    public void SelectSlot()
    {
        Inventory.m_instance.selectedSlotID = id;
        Inventory.m_instance.slotSelectedImage.transform.parent = this.transform;
        Inventory.m_instance.slotSelectedImage.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}
