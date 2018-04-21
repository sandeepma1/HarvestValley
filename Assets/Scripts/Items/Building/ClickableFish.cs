using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarvestValley.IO;
using System;
using DG.Tweening;

public class ClickableFish : ClickableBase
{
    public Fishes fish;
    public Action<int> OnFishClicked;
    private SpriteRenderer fishSprite;
    private Transform fishTransform;
    private DateTime nextSpawnDateTime;

    private Vector2 fishTravelRange = new Vector2(-1, 1);
    private float fishMinReactionTime = 0.35f;
    private float fishMaxReactionTime = 1f;

    private float fishMinDistance = 0.1f;
    private float fishMaxDistance = 1.5f;
    private Tween fishMove;
    private int randomfishDirection;

    private void Start()
    {
        Item fishItem = ItemDatabase.GetItemById(fish.itemId);
        gameObject.name = "Fish" + fish.itemId;
        fishSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        fishTransform = fishSprite.transform;
        fishSprite.sprite = AtlasBank.Instance.GetSprite(fishItem.slug, AtlasType.GUI);
        nextSpawnDateTime = DateTime.Parse(fish.nextSpawnDateTime);
        StartCoroutine("StartWandering");
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        OnFishClicked.Invoke(fish.itemId);
    }

    private IEnumerator StartWandering()
    {
        float reactionTime = UnityEngine.Random.Range(fishMinReactionTime, fishMaxReactionTime);
        fishMove = fishTransform.DOLocalMoveX(GetNextMove(), reactionTime).SetEase(Ease.InOutSine);
        fishMove = fishTransform.DOLocalMoveY(GetNextMove() / 3, reactionTime).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(reactionTime);
        StartCoroutine("StartWandering");
    }

    private float GetNextMove()
    {
        int recursiveCounter = 5;
        float localPosX = 0;

        randomfishDirection = UnityEngine.Random.Range(0, 2);
        float randomSteps = UnityEngine.Random.Range(fishMinDistance, fishMaxDistance);

        switch (randomfishDirection)
        {
            case 0://left
                localPosX = fishTransform.localPosition.x - randomSteps;
                fishSprite.flipX = false;
                break;
            case 1://right
                localPosX = fishTransform.localPosition.x + randomSteps;
                fishSprite.flipX = true;
                break;
        }

        if (localPosX > fishTravelRange.x && localPosX < fishTravelRange.y)
        {
            return localPosX;
        }
        else
        {
            recursiveCounter--;
            if (recursiveCounter < 0)
            {
                return fishTransform.localPosition.x;
            }
            GetNextMove();
        }
        return fishTransform.localPosition.x;
    }
}