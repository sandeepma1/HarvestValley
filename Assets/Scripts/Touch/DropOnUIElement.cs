using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropOnUIElement : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        switch (eventData.pointerDrag.tag)
        {
            case "DragableUiItem":

                break;
            default:
                //SeedListMenu.Instance.OnItemDropComplete();
                break;
        }
    }
}