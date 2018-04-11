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
    private float maxMoveAmount = 2;
    [SerializeField]
    private float minReactionTime = 0.5f;
    [SerializeField]
    private float maxReactionTime = 2;

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
        transform.DOLocalMove(GetNextMove(), 0.45f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(reactionTime);
        StartCoroutine("StartWandering");
    }

    private Vector2 GetNextMove()
    {
        Vector2 targetPos = Vector2.one;
        float randomDirectionX = transform.localPosition.x;
        float randomDirectionY = transform.localPosition.y;

        randomDirection = UnityEngine.Random.Range(0, 2);

        if (randomDirection == 0)
        {
            randomDirectionX = UnityEngine.Random.Range(randomDirectionX + maxMoveAmount, randomDirectionX - maxMoveAmount);
            if (randomDirectionX < 1)
            {
                randomDirectionX = 2;
            }
            else if (randomDirectionX > moveMaxX - 2) // for safer distance
            {
                randomDirectionX = moveMaxX - 3;
            }
            livestockSprite.sprite = randomDirectionX > transform.localPosition.x ? livestockRight : livestockLeft;
        }
        else
        {
            randomDirectionY = UnityEngine.Random.Range(randomDirectionY + maxMoveAmount, randomDirectionY - maxMoveAmount);
            if (randomDirectionY >= 0)
            {
                randomDirectionY = -2;
            }
            else if (randomDirectionY < moveMaxY + 2) // for safer distance
            {
                randomDirectionY = moveMaxY + 3;
            }
            livestockSprite.sprite = randomDirectionY > transform.localPosition.y ? livestockUp : livestockDown;
        }

        //if (randomDirectionX <= 0 || randomDirectionY >= 0)
        //{
        //    targetPos = new Vector3(2, -2);
        //    return targetPos;
        //}

        //if (randomDirectionX > moveMaxX || randomDirectionY < moveMaxY)
        //{
        //    targetPos = new Vector3(moveMaxX - 2, moveMaxY + 2);
        //    return targetPos;
        //}

        targetPos = new Vector3(randomDirectionX, randomDirectionY);
        return targetPos;
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
                LayHatchedOutput();
                livestock.biteCount = 0;
            }
        }
    }

    private void LayHatchedOutput()
    {
        GameObject output = Instantiate(Resources.Load("LivestockOutput") as GameObject);

        output.transform.parent = LivestockManager.Instance.transform;
        output.transform.localPosition = transform.localPosition;
        output.GetComponent<LivestockOutput>().CreateOutput(itemCanProduce.itemID);
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