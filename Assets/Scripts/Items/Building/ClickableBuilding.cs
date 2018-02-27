using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class ClickableBuilding : MouseUpBase
{
    public SpriteRenderer buildingSprite;
    public bool isSelected = false;
    public bool isDraggable = false;
    public int buildingID;
    public int sourceID;
    public BuildingState state;
    public int itemID;
    public System.DateTime dateTime;
    public static Action<int, int> OnBuildingClicked;
    public static Action<int> OnFieldHarvested;
    private int itemIDToBePlaced = -1;

    private void Start()
    {
        buildingSprite = GetComponent<SpriteRenderer>();
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        if (OnBuildingClicked != null)
        {
            OnBuildingClicked.Invoke(buildingID, sourceID);
        }
    }

    private void PlantSeed()
    {
        itemID = itemIDToBePlaced;
        dateTime = UTC.time.liveDateTime.AddMinutes(ItemDatabase.Instance.items[itemIDToBePlaced].timeRequiredInMins);
        state = BuildingState.WORKING;
        string plantName = ItemDatabase.Instance.items[itemIDToBePlaced].slug + "_0";
        //plantsSprite.sprite = AtlasBank.Instance.GetSprite(plantName, AtlasType.Farming);
        PlayerProfileManager.Instance.PlayerCoins(-ItemDatabase.Instance.items[itemIDToBePlaced].coinCost);
        StopPlantingMode();
        // TODO: Save on disk
    }

    internal void StartPlantingMode(int itemID)
    {
        itemIDToBePlaced = itemID;
    }

    internal void StopPlantingMode()
    {
        itemIDToBePlaced = -1;
    }
}
