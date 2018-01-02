using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBuildings : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IDragHandler
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

    public static event Action<int> OnClicked;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantsSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuildingsManager.Instance.CallParentOnMouseDown(id);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildingsManager.Instance.CallParentOnMouseEnter(id);
    }

    public void OnDrag(PointerEventData eventData)
    {
        BuildingsManager.Instance.CallParentOnMouseDrag(id);
    }

    public void TouchedUp()
    {
        if (OnClicked != null)
        {
            OnClicked.Invoke(id);
        }
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