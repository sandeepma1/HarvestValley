using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiFishingMenu : MonoBehaviour
{
    [SerializeField]
    private Image backgroundButton;
    [SerializeField]
    private RectTransform holdBarRectTransform;
    [SerializeField]
    private RectTransform progressBar;
    [SerializeField]
    private RectTransform dummyFishRectTransform;
    [SerializeField]
    private Button button;
    [SerializeField]
    private float upForce = 200;
    [SerializeField]
    private float progressFactor = 0.01f;
    [SerializeField]
    private float progressRedcutionMultiplier = 1.5f;
    [SerializeField]
    private Vector2 dummyFishTravelRange = new Vector2(60, 940);
    [SerializeField]
    private float dummyFishMinReactionTime = 0.15f;
    [SerializeField]
    private float dummyFishMaxReactionTime = 2f;


    private FishingHoldBarCollider holdBarScript;
    private float catchingProgress;
    private Rigidbody2D holdBarRigidBody;
    private bool isInFishingMode = true;
    private int randomDummyFishDirection;

    private void Start()
    {
        button.onClick.AddListener(OnPointerDown);
        holdBarRigidBody = holdBarRectTransform.GetComponent<Rigidbody2D>();
        holdBarScript = holdBarRectTransform.GetComponent<FishingHoldBarCollider>();
        holdBarRectTransform.GetComponent<BoxCollider2D>().size = holdBarRectTransform.sizeDelta;
        //StartCoroutine("StartWandering");
    }

    private void LateUpdate()
    {
        if (!isInFishingMode)
        {
            return;
        }
        if (holdBarScript.isOnStay)
        {
            UpdateProgressBar(progressFactor);
        }
        else
        {
            UpdateProgressBar(-progressFactor * progressRedcutionMultiplier); // progress redcution multiplier
        }
    }

    private void OnPointerDown()
    {
        holdBarRigidBody.velocity = Vector2.zero;
        holdBarRigidBody.AddForce(new Vector2(0, upForce));
    }

    private void UpdateProgressBar(float progressAdder)
    {
        catchingProgress += progressAdder;
        catchingProgress = Mathf.Clamp(catchingProgress, 0, 1);
        progressBar.localScale = new Vector3(progressBar.localScale.x, catchingProgress, progressBar.localScale.z);
    }

    private IEnumerator StartWandering()
    {
        float reactionTime = UnityEngine.Random.Range(dummyFishMinReactionTime, dummyFishMaxReactionTime);
        dummyFishRectTransform.DOLocalMoveY(GetNextMove(), reactionTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(reactionTime);
        //StartCoroutine("StartWandering");
    }

    private float GetNextMove()
    {
        print("next moce");
        int recursiveCounter = 5;
        float localPosY = dummyFishRectTransform.anchoredPosition.y;

        randomDummyFishDirection = UnityEngine.Random.Range(0, 2);
        float randomSteps = UnityEngine.Random.Range(dummyFishMinReactionTime, dummyFishMaxReactionTime);

        switch (randomDummyFishDirection)
        {
            case 0://up
                localPosY = dummyFishRectTransform.localPosition.y + randomSteps;
                break;
            case 1://down
                localPosY = dummyFishRectTransform.localPosition.y - randomSteps;
                break;
            default:
                break;
        }

        if (localPosY > dummyFishTravelRange.x && localPosY < dummyFishTravelRange.y)
        {
            return localPosY;
        }
        else
        {
            recursiveCounter--;
            if (recursiveCounter < 0)
            {
                return dummyFishRectTransform.anchoredPosition.y;
            }
            GetNextMove();
        }
        return dummyFishRectTransform.anchoredPosition.y;
    }
}