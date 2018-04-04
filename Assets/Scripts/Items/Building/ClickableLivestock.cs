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
    private int grassIdToEat = -1;
    private int grassAmountToEat = -1;
    private Item itemCanProduce;
    private int timeperBiteInSeconds;

    public event Action SaveLivestock;

    private void Start()
    {
        itemCanProduce = ItemDatabase.GetItemById(livestock.canProduceItemId);
        grassIdToEat = itemCanProduce.needID[0];
        grassAmountToEat = itemCanProduce.needAmount[0];
        timeperBiteInSeconds = itemCanProduce.timeRequiredInSeconds;
        print("timeperBiteInSeconds" + timeperBiteInSeconds);
        for (int i = 0; i < GrassLandManager.Instance.GetGrassCountBuyId(grassIdToEat); i++) // Todo: Maintain counter in GrassLandManager
        {
            CheckForRegularUpdates();
        }
        InvokeRepeating("CheckForRegularUpdates", 0, 1f);
    }

    private void CheckForRegularUpdates()
    {
        DateTime tempDateTime = dateTime.AddSeconds(timeperBiteInSeconds);
        if (tempDateTime <= DateTime.Now)
        {
            print("tick");
            dateTime = tempDateTime;
            //save datetime
            if (livestock.hatched < livestock.maxHatchCount)
            {
                if (GrassLandManager.Instance.GetGrassCountBuyId(grassIdToEat) > 0)
                {
                    GrassLandManager.Instance.RemoveGrass(grassIdToEat);
                    livestock.biteCount++;
                }
                if (livestock.biteCount >= grassAmountToEat)// Wola!! lay eggs/milk etc
                {
                    livestock.hatched++;
                    livestock.biteCount = 0;
                    print("egglayed");
                }
            }
            SaveLivestock.Invoke();
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