using System;
using System.Collections;
using UnityEngine;
using HarvestValley.IO;
using HarvestValley.Managers;
using HarvestValley.Ui;

public class ClickableLivestock : MouseUpBase
{
    public LivestockClass livestock;
    public DateTime dateTime;

    //temp variables to get faster from livestock above declared
    private int itemId = -1;
    private int itemNeeded = -1;
    private Item itemCanProduce;

    private void Start()
    {
        itemCanProduce = ItemDatabase.GetItemById(livestock.canProduceItemId);
        itemId = itemCanProduce.needID[0];
        itemNeeded = itemCanProduce.needAmount[0];
        InitLivestock();
        InvokeRepeating("CheckForHatching", 2, 1);
        print(livestock.livestockType + " created");
    }

    private void InitLivestock()
    {
        switch (livestock.livestockState)
        {
            case LivestockState.Idle:
                StartEating();
                break;
            case LivestockState.WaitingForGrass:
                WaitingForGrass();
                break;
            case LivestockState.Eating:
                break;
            case LivestockState.WaitingForHatch:
                break;
        }
    }

    private void CheckForHatching()
    {
        if (livestock.livestockState == LivestockState.Eating)
        {
            if (dateTime <= DateTime.UtcNow)
            {
                ReadyForHatch();
            }
        }
    }

    private void ReadyForHatch()
    {
        livestock.livestockState = LivestockState.WaitingForHatch;
        print("Ready to hatch");
    }

    private void StartEating()
    {
        if (GrassLandManager.Instance.IsAvailableGrassCount(itemId, itemNeeded)) // This will check and remove grass if true
        {
            dateTime = DateTime.UtcNow.AddSeconds(itemCanProduce.timeRequiredInSeconds);
            livestock.livestockState = LivestockState.Eating;
        }
        else
        {
            livestock.livestockState = LivestockState.WaitingForGrass;
            WaitingForGrass();
        }
    }

    private void WaitingForGrass()
    {
        StartCoroutine(CheckForAvailableGrass());
    }

    IEnumerator CheckForAvailableGrass() // Waiting for grass
    {
        print("Waiting for grass");
        yield return new WaitForSeconds(2);
        if (GrassLandManager.Instance.IsAvailableGrassCount(itemId, itemNeeded))
        {
            StartEating();
        }
        else
        {
            StartCoroutine(CheckForAvailableGrass());
        }
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        if (livestock.livestockState == LivestockState.WaitingForHatch)
        {
            livestock.livestockState = LivestockState.Idle;
            UiInventoryMenu.Instance.UpdateItems(livestock.canProduceItemId, 1);
            StartEating();
            print("egg added in inventory");
            //TODO: check if inventory is full or not
        }
        else
        {
            //TODO: On any livestock click like making happy
        }
    }
}