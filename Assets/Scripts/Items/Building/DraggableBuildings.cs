using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBuildings : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public bool isSelected = false;
    public bool isDraggable = false;
    public int id;
    public SpriteRenderer spriteRenderer;
    public int buildingID;
    public Vector2 pos;
    public int level;
    public BUILDINGS_STATE state;
    public int unlockedQueueSlots;
    public int itemID;
    public System.DateTime dateTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void OnClickDrag()
    {
        if (!IsPointerOverUIObject())
        {
            BuildingsManager.Instance.CallParentOnMouseDrag(id);
        }
        //if (!EventSystem.current.IsPointerOverGameObject())
        //{

        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GEM.isSwipeEnable = false;
        BuildingsManager.Instance.CallParentOnMouseDown(id);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        BuildingsManager.Instance.CallParentOnMouseUp(id);
        GEM.isSwipeEnable = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GEM.isSwipeEnable = false;
        BuildingsManager.Instance.CallParentOnMouseEnter(id);
    }
}

