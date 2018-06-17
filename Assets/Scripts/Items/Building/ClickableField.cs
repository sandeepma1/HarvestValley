using UnityEngine;
using DG.Tweening;
using HarvestValley.Managers;
using System;
using HarvestValley.Ui;
using HarvestValley.IO;

public class ClickableField : MonoBehaviour
{
    public int itemId;
    public DateTime dateTime;

    public SpriteRenderer plantsSprite;
    public bool isCrowPresent;

    public SpriteRenderer fieldSprite;
    private bool inPlantingMode;
    private Tweener glowingTweener = null;

    public int buildingId;
    public int sourceId;
    public Vector2 position;
    public int level;
    public BuildingState state;

    protected int itemIDToBePlaced = -1;

    private void Awake()
    {
        fieldSprite = GetComponent<SpriteRenderer>();
    }

    public void ShowCropMenu()
    {
        FieldManager.Instance.OnBuildingClicked(buildingId, sourceId);
        print(state);
    }

    public void SomeSeedPlanted()
    {
        if (inPlantingMode && state == BuildingState.IDLE)
        {
            AddItemToProductionQueue(itemIDToBePlaced);
            return;
        }
        FieldManager.Instance.OnBuildingClicked(buildingId, sourceId);
    }

    public void AddItemToProductionQueue(int itemId)
    {
        this.itemId = itemId;
        dateTime = DateTime.Now.AddSeconds(ItemDatabase.GetItemById(itemId).timeRequiredInSeconds);
        state = BuildingState.WORKING;

        string plantName = ItemDatabase.GetItemById(itemId).slug + "_0";
        plantsSprite.sprite = AtlasBank.Instance.GetSprite(plantName, AtlasType.Farming);
        PlayerProfileManager.Instance.PlayerCoins(-ItemDatabase.GetItemById(itemId).coinCost);
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
        glowingTweener = fieldSprite.DOColor(ColorConstants.FieldGlow, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopGlowing()
    {
        if (glowingTweener == null) { return; }

        glowingTweener.Kill();
        glowingTweener = null;
        fieldSprite.color = ColorConstants.FieldNormal;
    }
}