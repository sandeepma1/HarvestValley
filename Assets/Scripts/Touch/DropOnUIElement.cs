using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropOnUIElement : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        switch (eventData.pointerDrag.name)
        {
            case "ScytheImage":
                DraggableUIHarvesting.Instance.OnHarvestComplete();
                print("drop");
                break;
            default:
                UIMasterMenuManager.Instance.OnItemDropComplete();
                break;
        }
    }
}