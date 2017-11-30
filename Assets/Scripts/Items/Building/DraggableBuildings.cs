using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBuildings : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IDragHandler, IPointerExitHandler
{
    public bool isSelected = false;
    public bool isDraggable = false;
    public int id;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer plantsSprite;
    public int buildingID;
    public Vector2 pos;
    public int level;
    public BUILDINGS_STATE state;
    public int unlockedQueueSlots;
    public int itemID;
    public System.DateTime dateTime;

    private bool isOnObject;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantsSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GEM.isSwipeEnable = false;
        BuildingsManager.Instance.CallParentOnMouseDown(id);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isOnObject)
        {
            BuildingsManager.Instance.CallParentOnMouseUp(id);
            GEM.isSwipeEnable = true;
        } else
        {
            isOnObject = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnObject = true;
        GEM.isSwipeEnable = false;
        BuildingsManager.Instance.CallParentOnMouseEnter(id);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GEM.isSwipeEnable == false)
        {
            BuildingsManager.Instance.CallParentOnMouseDrag(id);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnObject = false;
    }
}

