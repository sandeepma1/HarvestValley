﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DraggableGrass : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IDragHandler
{
    internal int id;
    public int buildingID;
    internal int grassTypeID;
    internal SpriteRenderer grassSprite;
    internal System.DateTime dateTime;
    internal BUILDINGS_STATE state;
    internal bool isSelected;
    public static event Action<int> OnClicked;

    void Start()
    {
        grassSprite = GetComponent<SpriteRenderer>();
        //foreach (Transform child in transform)
        //{
        //    child.localPosition = Random.insideUnitCircle / 2;
        //    // child.GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.localPosition.y * -10);
        //}
    }

    public void TouchedUp()
    {
        // GEM.isDragging = false;
        //BuildingsManager.Instance.CallParentOnMouseUp(id);
        if (OnClicked != null)
        {
            OnClicked.Invoke(id);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //BuildingsManager.Instance.CallParentOnMouseDown(id);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //BuildingsManager.Instance.CallParentOnMouseEnter(id);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // BuildingsManager.Instance.CallParentOnMouseDrag(id);
    }

}
