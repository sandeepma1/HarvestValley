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
    [SerializeField]
    private float minMoveAmount = 1.5f;
    [SerializeField]
    private float maxMoveAmount = 3.5f;
    [SerializeField]
    private float minReactionTime = 0.5f;
    [SerializeField]
    private float maxReactionTime = 2;
    [SerializeField]
    private float movingSpeed = 1;

    //temp variables to get faster from livestock above declared
    private int grassIdToEat = -1;
    private int grassAmountToEat = -1;
    private Item itemCanProduce;
    private int timePerBiteInSeconds;
    private bool isThisAtStart;
    private DateTime tempDateTime;
    private int randomDirection;
    private SpriteRenderer livestockSprite;
    private SourceInfo sourceInfo;
    private Rect rect;
    //Images of every direction
    private Sprite livestockLeft;
    private Sprite livestockRight;
    private Sprite livestockUp;
    private Sprite livestockDown;

    private void Start()
    {
        livestockSprite = GetComponent<SpriteRenderer>();
        tempDateTime = DateTime.Parse(livestock.dateTime);
        itemCanProduce = ItemDatabase.GetItemById(livestock.canProduceItemId);
        grassIdToEat = itemCanProduce.needID[0];
        grassAmountToEat = itemCanProduce.needAmount[0];
        timePerBiteInSeconds = itemCanProduce.timeRequiredInSeconds;
        rect = new Rect(0, 0, moveMaxX - 2, moveMaxY + 2);
        StartCoroutine("WaitEndOfFrame");
    }

    private void LateUpdate()
    {
        livestockSprite.sortingOrder = (int)(transform.localPosition.y * -10) + 11;
    }

    private IEnumerator WaitEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        SlowStart();
    }

    private void SlowStart()
    {
        isThisAtStart = true;
        for (int i = 0; i < GrassLandManager.Instance.GetGrassCountBuyId(grassIdToEat); i++) // Todo: Maintain counter in GrassLandManager
        {
            CheckForRegularUpdates();
        }
        //lay hatched eggs again in-case user has not picked it.
        if (livestock.maxHatchCount > 1)
        {
            for (int i = 0; i < livestock.hatched; i++)
            {
                LayHatchedOutput();
            }
        }
        isThisAtStart = false;

        sourceInfo = SourceDatabase.GetSourceInfoById(livestock.sourceId);
        GetAllImages();
        SetCircleColliderSize();
        SetSpwanPosition();
        StartCoroutine("StartWandering");
        InvokeRepeating("CheckForRegularUpdates", 0, 0.5f);
    }

    private void SetCircleColliderSize()
    {
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        switch (sourceInfo.sourceID)
        {
            case 3: //chicken
                circleCollider2D.radius = 0.5f;
                break;
            case 5: // cow
                circleCollider2D.radius = 1f;
                break;
            default:
                break;
        }
    }

    private void GetAllImages()
    {
        livestockLeft = AtlasBank.Instance.GetSprite(sourceInfo.slug + "_left", AtlasType.Livestock);
        livestockRight = AtlasBank.Instance.GetSprite(sourceInfo.slug + "_right", AtlasType.Livestock);
        livestockUp = AtlasBank.Instance.GetSprite(sourceInfo.slug + "_up", AtlasType.Livestock);
        livestockDown = AtlasBank.Instance.GetSprite(sourceInfo.slug + "_down", AtlasType.Livestock);
        livestockSprite.sprite = livestockDown;
    }

    private void SetSpwanPosition()
    {
        transform.localPosition = new Vector2(5, -7);
    }

    private IEnumerator StartWandering()
    {
        float reactionTime = UnityEngine.Random.Range(minReactionTime, maxReactionTime);
        transform.DOLocalMove(GetNextMove(), movingSpeed).SetEase(Ease.Linear);
        yield return new WaitForSeconds(reactionTime);
        StartCoroutine("StartWandering");
    }

    private Vector2 GetNextMove()
    {
        int recursiveCounter = 5;
        Vector2 localPos = transform.localPosition;

        randomDirection = UnityEngine.Random.Range(0, 5);
        float randomSteps = UnityEngine.Random.Range(minMoveAmount, maxMoveAmount);
        switch (randomDirection)
        {
            case 0: //left
                localPos = new Vector3(transform.localPosition.x - randomSteps, transform.localPosition.y);
                livestockSprite.sprite = livestockLeft;
                break;
            case 1: //right
                localPos = new Vector3(transform.localPosition.x + randomSteps, transform.localPosition.y);
                livestockSprite.sprite = livestockRight;
                break;
            case 2://up
                localPos = new Vector3(transform.localPosition.x, transform.localPosition.y + randomSteps);
                livestockSprite.sprite = livestockUp;
                break;
            case 3://down
                localPos = new Vector3(transform.localPosition.x, transform.localPosition.y - randomSteps);
                livestockSprite.sprite = livestockDown;
                break;
            default:
                break;
        }
        if (rect.Contains(localPos, true))
        {
            return localPos;
        }
        else
        {
            recursiveCounter--;
            if (recursiveCounter < 0)
            {
                return transform.localPosition;
            }
            GetNextMove();
        }
        return transform.localPosition;
    }

    private void CheckForRegularUpdates()
    {
        if (livestock.hatched >= livestock.maxHatchCount)
        {
            return;
        }

        if (GrassLandManager.Instance.GetGrassCountBuyId(grassIdToEat) <= 0)
        {
            return;
        }

        if (tempDateTime <= DateTime.Now)
        {
            //Good for removing and eating grass closest to the livestock
            //Vector2 nearestFoodPos = GrassLandManager.Instance.GetNearestGrass(grassIdToEat, transform.localPosition); 
            //transform.DOLocalMove(nearestFoodPos, 0.45f).SetEase(Ease.Linear);

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
                if (livestock.maxHatchCount > 1)
                {
                    LayHatchedOutput();
                }
                else
                {
                    print("Grow fat to milk you");
                }
                livestock.biteCount = 0;
            }
        }
    }

    private void LayHatchedOutput()
    {
        GameObject output = Instantiate(Resources.Load("LivestockOutput") as GameObject);

        output.transform.parent = LivestockManager.Instance.transform;
        output.transform.localPosition = transform.localPosition; // Todo place at random at game load
        output.GetComponent<LivestockOutput>().CreateOutput(itemCanProduce.itemID);
        output.GetComponent<LivestockOutput>().ItemCollected += ItemCollectedEventHandler;
    }

    private void ItemCollectedEventHandler()
    {
        livestock.hatched--;
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        if (livestock.maxHatchCount == 1 && livestock.hatched == 1)
        {
            UiInventoryMenu.Instance.UpdateItems(livestock.canProduceItemId, livestock.hatched);
            livestock.hatched = 0;
            print("added milk in inventory " + livestock.hatched);
        }
    }
}