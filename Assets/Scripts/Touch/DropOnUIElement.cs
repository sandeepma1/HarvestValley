using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropOnUIElement : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        UIMasterMenuManager.Instance.OnDropComplete();
    }
}