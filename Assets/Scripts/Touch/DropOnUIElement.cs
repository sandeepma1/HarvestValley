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

                break;
            default:
                UIMasterMenuManager.Instance.OnItemDropComplete();
                break;
        }
    }
}