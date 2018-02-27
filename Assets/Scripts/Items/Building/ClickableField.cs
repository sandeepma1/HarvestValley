﻿using System;
using UnityEngine;
using DG.Tweening;

public class ClickableField : MouseUpBase
{
    public SpriteRenderer fieldSprite;
    public SpriteRenderer plantsSprite;
    public GameObject crowGO;
    public bool isSelected = false;
    public bool isDraggable = false;
    public int fieldID;
    public int sourceID;
    public Vector2 pos;
    public int level;
    public FieldState state;
    public int unlockedQueueSlots;
    public int itemID;
    public System.DateTime dateTime;
    public bool isCrowPresent;
    public static Action<int, int> OnFieldClicked;
    public static Action<int> OnFieldHarvested;

    internal int baseYieldMin;
    internal int baseYieldMax;
    internal int noOfWatering;

    //private SpriteRenderer fieldSprite;
    private bool inPlantingMode;
    private Tweener glowingTweener = null;
    private int itemIDToBePlaced = -1;

    private void Awake()
    {
        fieldSprite = GetComponent<SpriteRenderer>();
    }

    public void CrowComes()
    {
        crowGO.SetActive(true);
    }

    public void CrowGoes()
    {
        crowGO.SetActive(false);
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        if (inPlantingMode && state == FieldState.NONE)
        {
            PlantSeed();
            return;
        }
        if (OnFieldClicked != null)
        {
            OnFieldClicked.Invoke(fieldID, sourceID);
        }
    }

    private void PlantSeed()
    {
        itemID = itemIDToBePlaced;
        dateTime = UTC.time.liveDateTime.AddMinutes(ItemDatabase.Instance.items[itemIDToBePlaced].timeRequiredInMins);
        state = FieldState.GROWING;
        string plantName = ItemDatabase.Instance.items[itemIDToBePlaced].slug + "_0";
        plantsSprite.sprite = AtlasBank.Instance.GetSprite(plantName, AtlasType.Farming);
        PlayerProfileManager.Instance.PlayerCoins(-ItemDatabase.Instance.items[itemIDToBePlaced].coinCost);
        StopPlantingMode();
        // TODO: Save on disk
    }

    internal void StartPlantingMode(int itemID)
    {
        StopGlowing();
        inPlantingMode = true;
        itemIDToBePlaced = itemID;
        StartGlowing();
    }

    internal void StopPlantingMode()
    {
        if (inPlantingMode)
        {
            inPlantingMode = false;
            itemIDToBePlaced = -1;
            StopGlowing();
        }
    }

    private void StartGlowing()
    {
        glowingTweener = fieldSprite.DOColor(ColorConstants.fieldGlow, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopGlowing()
    {
        if (glowingTweener != null)
        {
            glowingTweener.Kill();
            glowingTweener = null;
            fieldSprite.color = ColorConstants.fieldNormal;
        }
    }
}