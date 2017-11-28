using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBuildings : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public bool isSelected = false;
    public bool isDraggable = false;
    public int id;
    public int buildingID;
    public Vector2 pos;
    public int level;
    public BUILDINGS_STATE state;
    public int unlockedQueueSlots;
    public int itemID;
    public System.DateTime dateTime;
    //public string s_dateTime = "";

    public void OnClickDrag()
    {
        //if (!EventSystem.current.IsPointerOverGameObject())
        //{
        BuildingsManager.m_instance.CallParentOnMouseDrag(id);
        // }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GEM.isSwipeEnable = false;
        BuildingsManager.m_instance.CallParentOnMouseDown(id);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        BuildingsManager.m_instance.CallParentOnMouseUp(id);
        GEM.isSwipeEnable = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GEM.isSwipeEnable = false;
        BuildingsManager.m_instance.CallParentOnMouseEnter(id);
    }
}

