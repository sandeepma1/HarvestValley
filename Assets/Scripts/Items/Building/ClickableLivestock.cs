using System;
using System.Collections;
using UnityEngine;
using HarvestValley.IO;
using HarvestValley.Managers;
using HarvestValley.Ui;

public class ClickableLivestock : MouseUpBase
{
    public LivestockClass livestock;

    //temp variables to get faster from livestock above declared
    private int grassIdToEat = -1;
    private int grassAmountToEat = -1;
    private Item itemCanProduce;
    private int timePerBiteInSeconds;
    private bool isThisAtStart;
    private DateTime tempDateTime;

    private void Start()
    {
        tempDateTime = DateTime.Parse(livestock.dateTime);
        itemCanProduce = ItemDatabase.GetItemById(livestock.canProduceItemId);
        grassIdToEat = itemCanProduce.needID[0];
        grassAmountToEat = itemCanProduce.needAmount[0];
        timePerBiteInSeconds = itemCanProduce.timeRequiredInSeconds;
        StartCoroutine("WaitForEndOfFrame");
    }

    public IEnumerator WaitForEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        isThisAtStart = true;
        for (int i = 0; i < GrassLandManager.Instance.GetGrassCountBuyId(grassIdToEat); i++) // Todo: Maintain counter in GrassLandManager
        {
            CheckForRegularUpdates();
        }
        isThisAtStart = false;

        InvokeRepeating("CheckForRegularUpdates", 0, 0.5f);
    }

    private void CheckForRegularUpdates()
    {
        if (livestock.hatched > livestock.maxHatchCount)
        {
            return;
        }

        if (GrassLandManager.Instance.GetGrassCountBuyId(grassIdToEat) <= 0)
        {
            return;
        }

        if (tempDateTime <= DateTime.Now)
        {
            GrassLandManager.Instance.RemoveGrass(grassIdToEat);
            livestock.biteCount++;

            if (isThisAtStart)
            {
                tempDateTime = tempDateTime.AddSeconds(timePerBiteInSeconds);
            }
            else
            {
                tempDateTime = DateTime.Now.AddSeconds(timePerBiteInSeconds); // Add next bite time in seconds   
            }

            if (livestock.biteCount >= grassAmountToEat)// Wola!! lay eggs/milk etc
            {
                livestock.hatched++;
                livestock.biteCount = 0;
            }
        }
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        if (livestock.hatched == 0)
        {
            print("no eggs");
            return;
        }
        print("added eggs in inventory " + livestock.hatched);
        UiInventoryMenu.Instance.UpdateItems(livestock.canProduceItemId, livestock.hatched);
        livestock.hatched = 0;
    }
}