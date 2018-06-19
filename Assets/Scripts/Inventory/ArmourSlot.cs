﻿using UnityEngine;
using UnityEngine.EventSystems;

public class ArmourSlot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerDownHandler
{
    public int id;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemData droppedItem = eventData.pointerDrag.GetComponent<InventoryItemData>();
        print("amt " + droppedItem.GetComponent<InventoryItemData>().amount);
        if (droppedItem.type != ItemType.Armor)
        {
            return;
        }
        if (Inventory.m_instance.items[id].itemID == -1)
        {
            Inventory.m_instance.items[droppedItem.slotID] = new Item();
            Inventory.m_instance.items[id] = droppedItem.item;
            droppedItem.slotID = id;
        }
        else
        {
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
