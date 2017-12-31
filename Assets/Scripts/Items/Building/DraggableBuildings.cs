using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBuildings : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IDragHandler
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

    public void TouchedUp()
    {
        // GEM.isDragging = false;
        BuildingsManager.Instance.CallParentOnMouseUp(id);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantsSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuildingsManager.Instance.CallParentOnMouseDown(id);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //GEM.isSwipeEnable = false;
        BuildingsManager.Instance.CallParentOnMouseEnter(id);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // GEM.isDragging = true;
        BuildingsManager.Instance.CallParentOnMouseDrag(id);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "MasterMenuItem":
                BuildingsManager.Instance.CallParentOnMouseEnter(id);
                break;
            case "Harvest":
                BuildingsManager.Instance.CallParentOnMouseEnter(id);
                break;
            default:
                break;
        }
    }
}

