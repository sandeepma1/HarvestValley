using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HarvestValley.Ui
{
    public class UiFishingMenu : UiMenuBase<UiFishingMenu>
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
        private float progressFactor = 0.0015f;
        [SerializeField]
        private float progressRedcutionMultiplier = 1.5f;
        [SerializeField]
        private Vector2 dummyFishTravelRange = new Vector2(60, 940);
        [SerializeField]
        private float dummyFishMinReactionTime = 0.15f;
        [SerializeField]
        private float dummyFishMaxReactionTime = 0.5f;
        [SerializeField]
        private float dummyFishMinDistance = 75;
        [SerializeField]
        private float dummyFishMaxDistance = 100;


        private FishingHoldBarCollider holdBarScript;
        private float catchingProgress = 0.15f;
        private Rigidbody2D holdBarRigidBody;
        private bool isInFishingMode = false;
        private int randomDummyFishDirection;
        private Tween dummyFishMove;

        private Vector3 holdBarDefaultPosition;
        private Vector3 dummyFishDefaultPosition;
        private float catchingProgressDefault = 0.05f;

        protected override void Start()
        {
            base.Start();
            holdBarDefaultPosition = holdBarRectTransform.anchoredPosition;
            dummyFishDefaultPosition = dummyFishRectTransform.anchoredPosition;
            button.onClick.AddListener(OnButtonClickEventHandler);
            holdBarRigidBody = holdBarRectTransform.GetComponent<Rigidbody2D>();
            holdBarScript = holdBarRectTransform.GetComponent<FishingHoldBarCollider>();
            holdBarRectTransform.GetComponent<BoxCollider2D>().size = holdBarRectTransform.sizeDelta;
        }

        public void StartFishingMode()
        {
            LoadDefaultStates();
            StartCoroutine("StartWandering");
            isInFishingMode = true;
        }

        private void LoadDefaultStates()
        {
            catchingProgress = catchingProgressDefault;
            holdBarRectTransform.anchoredPosition = holdBarDefaultPosition;
            dummyFishRectTransform.anchoredPosition = dummyFishDefaultPosition;
        }

        private void OnButtonClickEventHandler()
        {
            if (!isInFishingMode)
            {
                return;
            }
            holdBarRigidBody.velocity = Vector2.zero;
            holdBarRigidBody.AddForce(new Vector2(0, upForce));
        }

        private void LateUpdate()
        {
            if (!isInFishingMode)
            {
                return;
            }
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            if (holdBarScript.isOnStay)
            {
                UpdateProgressBarValues(progressFactor);
            }
            else
            {
                UpdateProgressBarValues(-progressFactor * progressRedcutionMultiplier);
            }
        }

        private void UpdateProgressBarValues(float progressAdder)
        {
            catchingProgress += progressAdder;
            catchingProgress = Mathf.Clamp(catchingProgress, 0, 1);
            progressBar.localScale = new Vector3(progressBar.localScale.x, catchingProgress, progressBar.localScale.z);
            if (catchingProgress == 1)
            {
                isInFishingMode = false;
                FishCaughut();
                StopFishing();
            }
            if (catchingProgress == 0)
            {
                isInFishingMode = false;
                StopFishing();
                YouLoose();
            }
        }

        private void StopFishing()
        {
            StopCoroutine("StartWandering");
            dummyFishMove.Kill();
        }

        private void FishCaughut()
        {
            // print("Got fish");
            MenuManager.Instance.CloseMenu();
        }

        private void YouLoose()
        {
            // print("No fish caught");
            MenuManager.Instance.CloseMenu();
        }

        #region Fish Wandering

        private IEnumerator StartWandering()
        {
            float reactionTime = UnityEngine.Random.Range(dummyFishMinReactionTime, dummyFishMaxReactionTime);
            //dummyFishRectTransform.DOLocalMoveY(GetNextMove(), reactionTime).SetEase(Ease.Linear);
            dummyFishMove = dummyFishRectTransform.DOAnchorPosY(GetNextMove(), reactionTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(reactionTime);
            StartCoroutine("StartWandering");
        }

        private float GetNextMove()
        {
            int recursiveCounter = 5;
            float localPosY = 0;

            randomDummyFishDirection = UnityEngine.Random.Range(0, 2);
            float randomSteps = UnityEngine.Random.Range(dummyFishMinDistance, dummyFishMaxDistance);
            switch (randomDummyFishDirection)
            {
                case 0://up
                    localPosY = dummyFishRectTransform.anchoredPosition.y + randomSteps;
                    break;
                case 1://down
                    localPosY = dummyFishRectTransform.anchoredPosition.y - randomSteps;
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

        #endregion
    }
}