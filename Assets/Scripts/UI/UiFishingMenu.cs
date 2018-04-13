using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HarvestValley.IO;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiFishingMenu : UiMenuBase<UiFishingMenu>
    {
        [SerializeField]
        private Image backgroundButton;
        [SerializeField]
        private GameObject miniFishingGameGO;
        [SerializeField]
        private GameObject fishCapturedUiGO;
        [SerializeField]
        private RectTransform holdBarRectTransform;
        [SerializeField]
        private RectTransform progressBar;
        [SerializeField]
        private RectTransform dummyFishRectTransform;
        [SerializeField]
        private Button clickerButton;
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
        // All Ui Components
        [SerializeField]
        private Image fishImage;
        [SerializeField]
        private TextMeshProUGUI fishNameText;
        [SerializeField]
        private TextMeshProUGUI fishWeightText;
        [SerializeField]
        private TextMeshProUGUI xpAmountOnCaughtText;
        [SerializeField]
        private TextMeshProUGUI filletAmountOnCaughtText;
        [SerializeField]
        private TextMeshProUGUI xpAmountOnReleaseText;
        [SerializeField]
        private Button catchFishButton;
        [SerializeField]
        private Button releaseFishButton;

        private bool isClicked;
        private FishingHoldBarCollider holdBarScript;
        private float catchingProgress = 0.15f;
        private Rigidbody2D holdBarRigidBody;
        private bool isInFishingMode = false;
        private int randomDummyFishDirection;
        private Tween dummyFishMove;

        private Vector3 holdBarDefaultPosition;
        private Vector3 dummyFishDefaultPosition;
        private float catchingProgressDefault = 0.05f;
        private int tempfishId = -1;
        private int tempFishFillet;
        private int tempXpOnCaught;
        private int tempXpOnRelease;

        protected override void Start()
        {
            base.Start();
            holdBarDefaultPosition = holdBarRectTransform.anchoredPosition;
            dummyFishDefaultPosition = dummyFishRectTransform.anchoredPosition;
            holdBarRigidBody = holdBarRectTransform.GetComponent<Rigidbody2D>();
            holdBarScript = holdBarRectTransform.GetComponent<FishingHoldBarCollider>();
            holdBarRectTransform.GetComponent<BoxCollider2D>().size = holdBarRectTransform.sizeDelta;
            releaseFishButton.onClick.AddListener(OnReleaseFishButtonPressedEventHandler);
            catchFishButton.onClick.AddListener(OnCatchFishButtonPressedEventHandler);
        }

        private void Update()
        {
            if (isInFishingMode && isClicked)
            {
                OnButtonClickAndHoldEventHandler();
            }
        }

        public void StartFishingMode(int fishId)
        {
            LoadDefaultStates();
            tempfishId = fishId;
            miniFishingGameGO.SetActive(true);
            fishCapturedUiGO.SetActive(false);
            backgroundButton.gameObject.SetActive(false);
            StartCoroutine("StartWandering");
            isInFishingMode = true;
        }

        private void StopFishingMode(bool isFishCaught)
        {
            isInFishingMode = false;
            StopCoroutine("StartWandering");
            backgroundButton.gameObject.SetActive(true);
            dummyFishMove.Kill();
            miniFishingGameGO.SetActive(false);
            if (isFishCaught)
            {
                fishCapturedUiGO.SetActive(true);
                PopulateFishCaughtUiItems();
            }
            else
            {
                CloseThisFishingMenu();
            }
        }

        private void PopulateFishCaughtUiItems()
        {
            Item fish = ItemDatabase.GetItemById(tempfishId);
            fishImage.sprite = AtlasBank.Instance.GetSprite(fish.slug, AtlasType.GUI);
            fishNameText.text = fish.name;

            float fishWeight = UnityEngine.Random.Range(1f, 5f);
            fishWeightText.text = fishWeight.ToString("F2") + " kg";

            int filletAmount = 0;
            if (fishWeight < 1.5f)
            {
                filletAmount = Mathf.RoundToInt((fishWeight / 1.5f));
            }
            else
            {
                filletAmount = (int)(fishWeight / 1.5f);
            }
            filletAmountOnCaughtText.text = "+ " + filletAmount.ToString();
            print(fishWeight + " " + filletAmount + " " + fishWeight / 1.5f);

            int xpOnCaught = fish.XPperYield;
            xpAmountOnCaughtText.text = "+ " + xpOnCaught.ToString();
            int xpOnRelease = xpOnCaught * 2;
            xpAmountOnReleaseText.text = "+ " + xpOnRelease.ToString();

            tempFishFillet = filletAmount;
            tempXpOnCaught = xpOnCaught;
            tempXpOnRelease = xpOnRelease;
        }

        private void OnCatchFishButtonPressedEventHandler()
        {
            PlayerProfileManager.Instance.PlayerXPPointsAdd(tempXpOnCaught);
            UiInventoryMenu.Instance.UpdateItems(9, tempFishFillet);
            CloseThisFishingMenu();
        }

        private void OnReleaseFishButtonPressedEventHandler()
        {
            PlayerProfileManager.Instance.PlayerXPPointsAdd(tempXpOnRelease);
            CloseThisFishingMenu();
        }

        private void CloseThisFishingMenu()
        {
            MenuManager.Instance.CloseMenu();
        }

        private void LoadDefaultStates()
        {
            tempfishId = -1;
            tempFishFillet = 0;
            tempXpOnCaught = 0;
            tempXpOnRelease = 0;
            catchingProgress = catchingProgressDefault;
            holdBarRectTransform.anchoredPosition = holdBarDefaultPosition;
            dummyFishRectTransform.anchoredPosition = dummyFishDefaultPosition;
        }

        public void DetectTapAndHold(bool flag)
        {
            if (isInFishingMode)
            {
                isClicked = flag;
            }
        }

        private void OnButtonClickAndHoldEventHandler()
        {
            holdBarRigidBody.velocity = Vector2.zero;
            holdBarRigidBody.AddForce(new Vector2(0, upForce));
        }

        public void OnButtonClickEventHandler()
        {
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
                StopFishingMode(true);
            }
            if (catchingProgress == 0)
            {
                StopFishingMode(false);
            }
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