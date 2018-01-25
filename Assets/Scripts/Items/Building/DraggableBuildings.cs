using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBuildings : MouseUpBase
{
    public SpriteRenderer buildingSprite;
    public SpriteRenderer plantsSprite;
    public GameObject crowGO;
    public bool isSelected = false;
    public bool isDraggable = false;
    public int buildingID;
    public int sourceID;
    public Vector2 pos;
    public int level;
    public BUILDINGS_STATE state;
    public int unlockedQueueSlots;
    public int itemID;
    public System.DateTime dateTime;
    public bool isCrowPresent;
    public static Action<int, int> OnBuildingClicked;
    public static Action<int> OnBuildingHarvested;

    internal int baseYieldMin;
    internal int baseYieldMax;
    internal int noOfWatering;

    public void CrowComes()
    {
        crowGO.SetActive(true);
    }

    public void CrowGoes()
    {
        crowGO.SetActive(false);
    }

    public override void TouchUp()
    {
        base.TouchUp();
        if (OnBuildingClicked != null)
        {
            OnBuildingClicked.Invoke(buildingID, sourceID);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == BUILDINGS_STATE.WAITING_FOR_HARVEST)
        {
            if (collision.CompareTag("Harvest"))
            {
                if (OnBuildingHarvested != null)
                {
                    OnBuildingHarvested.Invoke(buildingID);
                }
            }
        }
    }

    // Old Code Remove later


    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    BuildingsManager.Instance.CallParentOnMouseDown(buildingID);
    //}
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    BuildingsManager.Instance.CallParentOnMouseEnter(buildingID);
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    BuildingsManager.Instance.CallParentOnMouseDrag(buildingID);
    //}
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    switch (other.tag)
    //    {
    //        case "MasterMenuItem":
    //            BuildingsManager.Instance.CallParentOnMouseEnter(buildingID);
    //            break;
    //        case "Harvest":
    //            BuildingsManager.Instance.CallParentOnMouseEnter(buildingID);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}