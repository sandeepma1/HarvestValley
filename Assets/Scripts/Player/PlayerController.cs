using UnityEngine;
using HarvestValley.IO;
using System;
using DG.Tweening;

public class PlayerController : Singleton<PlayerController>
{
    private FloatingJoystick Joystick;
    public event Action<PickaxeAble> OnPickaxeAbleClicked;
    public event Action<string> OnEnteranceClicked;

    public bool isPlayerInAction;
    public ActionButtonType actionButtonType;
    public PlayerToObjectDirection playerToObjectDirection = PlayerToObjectDirection.None;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float radius = 1;

    private bool isMoving;
    public string actionTriggerColliderName;
    public string actionTagName;
    private Collider2D currentCollider2D;
    private Transform hitBox;
    private Transform nearestObject;
    private PlayerMovement playerMovement;
    private Vector3 direction;

    private bool ActionButtonVisiblilty { set { Joystick.actionButton.gameObject.SetActive(value); } }
    private bool SecondaryButtonVisiblilty { set { Joystick.secondaryButton.gameObject.SetActive(value); } }
    private string ActionString { get { return Joystick.actionText.text; } set { Joystick.actionText.text = value; } }
    private string SecondaryString { set { Joystick.secondaryText.text = value; } }

    #region Unity Default
    private void Start()
    {
        hitBox = transform.GetChild(0);
        Joystick = FindObjectOfType<FloatingJoystick>();
        ActionButtonSetActive(false);
        SecondaryButtonSetActive(false);
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.IfPlayerMoving += IfPlayerMovingEventhandler;

        Joystick.actionButton.OnHitComplete += OnActionButtonClickEventHandler;
        Joystick.secondaryButton.OnHitComplete += OnSecondaryButtonClickEventHandler;
    }

    private void IfPlayerMovingEventhandler(bool flag)
    {
        isMoving = flag;
    }

    protected override void OnDestroy()
    {
        Joystick.actionButton.OnHitComplete -= OnActionButtonClickEventHandler;
        Joystick.secondaryButton.OnHitComplete -= OnSecondaryButtonClickEventHandler;
        playerMovement.IfPlayerMoving -= IfPlayerMovingEventhandler;
    }
    #endregion

    #region Action Secondary Buttons Functions
    private void ActionButtonSetActive(bool flag)
    {
        ActionButtonVisiblilty = flag;
    }

    private void SecondaryButtonSetActive(bool flag)
    {
        SecondaryButtonVisiblilty = flag;
    }

    private void ActionButtonText(string text)
    {
        if (ActionString != text)
        {
            ActionString = text;
        }
    }

    private void SecondaryButtonText(string text)
    {
        SecondaryString = text;
    }
    #endregion

    #region Nearest Object Detection
    private void LateUpdate()
    {
        //if (isMoving)
        //{
        Collider2D[] groundOverlap = new Collider2D[4];
        Physics2D.OverlapCircleNonAlloc(transform.position, radius, groundOverlap, layerMask);
        if (GetClosestEnemy(groundOverlap) != null)
        {
            nearestObject = GetClosestEnemy(groundOverlap);
            NearestObject(nearestObject);
        }
        else
        {
            nearestObject = null;
            NoNearObject();
        }
        //}
    }

    private Transform GetClosestEnemy(Collider2D[] groundOverlap)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < groundOverlap.Length; i++)
        {
            if (groundOverlap[i] == null)
            {
                continue;
            }
            Vector3 directionToTarget = groundOverlap[i].transform.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = groundOverlap[i].transform;
            }
        }
        return bestTarget;
    }

    private void SetPlayerToObjectDirection(Vector3 direction)
    {
        if (direction.x > 0)
        {
            playerToObjectDirection = PlayerToObjectDirection.Left;
        }
        else
        {
            playerToObjectDirection = PlayerToObjectDirection.Right;
        }
    }
    #endregion

    #region Display Data on Action Secondary Buttons
    private void NearestObject(Transform collision)
    {
        SetPlayerToObjectDirection((transform.position - collision.position).normalized);
        UiDebugTextHandler.DebugText(playerToObjectDirection.ToString());
        actionTagName = collision.tag;
        actionTriggerColliderName = collision.name;
        actionButtonType = (ActionButtonType)System.Enum.Parse(typeof(ActionButtonType), actionTagName);
        ActionButtonSetActive(true);
        switch (actionTagName)
        {
            case "Enterence":
                ActionButtonText("Enter " + actionTriggerColliderName);
                ShowHitLoction(collision.position);
                break;
            case "Pickaxe":
                ActionButtonText("Pickaxe");
                ShowHitLoction(collision.position);
                break;
            case "DroppedItem":
                GetDroppedItem(collision);
                HideHitLocation();
                break;
            case "Ladder":
                ActionButtonText("Go Down");
                ShowHitLoction(collision.position);
                break;
            default:
                ActionButtonSetActive(false);
                break;
        }
    }

    private void NoNearObject()
    {
        ActionButtonSetActive(false);
        SecondaryButtonSetActive(false);
        HideHitLocation();
    }

    private void GetDroppedItem(Transform droppedItem)
    {
        int itemIdToAdd = droppedItem.GetComponent<DroppedItem>().itemId;
    }

    private void ShowHitLoction(Vector3 position)
    {
        hitBox.transform.position = position;
    }

    private void HideHitLocation()
    {
        hitBox.transform.position = new Vector3(500, 500);
    }

    #endregion

    #region Action Secondary buttons pressed
    private void OnActionButtonClickEventHandler()
    {
        isPlayerInAction = false;
        switch (actionTagName)
        {
            case "Enterence":
                OnEnteranceClicked.Invoke(actionTriggerColliderName);
                break;
            case "Pickaxe":
                PickaxeAble pickaxeAble = nearestObject.GetComponent<PickaxeAble>();
                OnPickaxeAbleClicked.Invoke(pickaxeAble);
                break;
            case "Ladder":
                print("next level");
                break;
            default:
                break;
        }
    }

    private void OnSecondaryButtonClickEventHandler()
    {
        SecondaryButtonSetActive(false);
    }
    #endregion
}

public enum PlayerToObjectDirection
{
    None,
    Left,
    Right
}