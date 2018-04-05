using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using HarvestValley.IO;
using HarvestValley.Managers;
using HarvestValley.Ui;

public class ClickableLivestock : MouseUpBase
{
    public LivestockClass livestock;
    [SerializeField]
    private int moveMaxX;
    [SerializeField]
    private int moveMaxY;
    //temp variables to get faster from livestock above declared
    private int grassIdToEat = -1;
    private int grassAmountToEat = -1;
    private Item itemCanProduce;
    private int timePerBiteInSeconds;
    private bool isThisAtStart;
    private DateTime tempDateTime;
    private int randDir = 1;
    private float moveSpeed = 2f;
    private SpriteRenderer livestockSprite;
    private Vector2 iniPosition;

    private void Start()
    {
        livestockSprite = GetComponent<SpriteRenderer>();
        tempDateTime = DateTime.Parse(livestock.dateTime);
        itemCanProduce = ItemDatabase.GetItemById(livestock.canProduceItemId);
        grassIdToEat = itemCanProduce.needID[0];
        grassAmountToEat = itemCanProduce.needAmount[0];
        timePerBiteInSeconds = itemCanProduce.timeRequiredInSeconds;
        StartCoroutine("WaitForEndOfFrame");
    }

    private void LateUpdate()
    {
        livestockSprite.sortingOrder = (int)(transform.localPosition.y * -10) + 11;
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
        SetSpwanPosition();
        InvokeRepeating("CheckForRegularUpdates", 0, 0.5f);
        StartCoroutine("StartWandering");
    }

    private void SetSpwanPosition()
    {
        iniPosition = new Vector2(3, 2);
    }
    private IEnumerator StartWandering()
    {
        // transform.localPosition = GetNextMove();
        transform.DOLocalMove(GetNextMove(), 1.5f);
        yield return new WaitForSeconds(3);
        StartCoroutine("WaitForEndOfFrame");
    }

    private Vector2 GetNextMove()
    {
        float randomRange = 1.5f;
        float x = UnityEngine.Random.Range(iniPosition.x - randomRange, iniPosition.x + randomRange);
        float y = UnityEngine.Random.Range(iniPosition.y - randomRange, iniPosition.y + randomRange);
        if (x < 1 || y < 1)
        {
            iniPosition = Vector2.one;
            return iniPosition;
        }

        if (x > moveMaxX || y > moveMaxY)
        {
            iniPosition = new Vector2(iniPosition.x - 1, iniPosition.y - 1);
            return iniPosition;
        }
        iniPosition = new Vector2(x, -y);
        return iniPosition;
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