using UnityEngine;
using DG.Tweening;
using HarvestValley.Managers;
using System;
using HarvestValley.Ui;

public class ClickableField : ClickableBase
{
    public int itemId;
    public DateTime dateTime;

    public SpriteRenderer plantsSprite;
    public bool isCrowPresent;

    private bool inPlantingMode;
    private Tweener glowingTweener = null;

    private void Awake()
    {
        buildingSprite = GetComponent<SpriteRenderer>();
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        if (inPlantingMode && state == BuildingState.IDLE)
        {
            AddItemToProductionQueue(itemIDToBePlaced);
            return;
        }
        FieldManager.Instance.OnBuildingClicked(buildingId, sourceId);
    }

    public override void AddItemToProductionQueue(int itemId)
    {
        base.AddItemToProductionQueue(itemId);
        this.itemId = itemId;
        dateTime = DateTime.UtcNow.AddSeconds(ItemDatabase.Instance.items[itemId].timeRequiredInSeconds);
        state = BuildingState.WORKING;

        string plantName = ItemDatabase.Instance.items[itemId].slug + "_0";
        plantsSprite.sprite = AtlasBank.Instance.GetSprite(plantName, AtlasType.Farming);
        PlayerProfileManager.Instance.PlayerCoins(-ItemDatabase.Instance.items[itemId].coinCost);
        StopPlantingMode();
    }

    internal void StartPlantingMode(int itemId)
    {
        StopGlowing();
        inPlantingMode = true;
        itemIDToBePlaced = itemId;
        StartGlowing();
    }

    internal void StopPlantingMode()
    {
        if (!inPlantingMode) { return; }

        inPlantingMode = false;
        itemIDToBePlaced = -1;
        StopGlowing();
    }

    private void StartGlowing()
    {
        glowingTweener = buildingSprite.DOColor(ColorConstants.FieldGlow, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopGlowing()
    {
        if (glowingTweener == null) { return; }

        glowingTweener.Kill();
        glowingTweener = null;
        buildingSprite.color = ColorConstants.FieldNormal;
    }
}