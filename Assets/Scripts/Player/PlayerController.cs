using UnityEngine;
using HarvestValley.IO;
using System;
using DG.Tweening;

public class PlayerController : Singleton<PlayerController>
{
    private FloatingJoystick Joystick;
    public event Action<PickaxeAble> OnPickaxeAbleClicked;
    public event Action<EnteranceType, int> OnEnteranceClicked;
    public event Action<OpenMenuTypes> OnOpenMenuClicked;

    public PlayerToObjectDirection playerToObjectDirection = PlayerToObjectDirection.None;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float radius = 1;

    public bool isPlayerInAction;
    private bool isMoving;
    private bool isActionButtonOnHold;
    public string actionTriggerColliderName;
    public InteractiveItemType itemType;
    private Collider2D currentCollider2D;
    private Transform hitBox;
    private InteractiveItem nearestItem;
    private PlayerMovement playerMovement;
    private Vector3 direction;

    private bool ActionButtonVisiblilty { set { Joystick.actionButton.gameObject.SetActive(value); } }
    private string ActionString { get { return Joystick.actionText.text; } set { Joystick.actionText.text = value; } }

    #region Unity Default
    private void Start()
    {
        hitBox = transform.GetChild(0);
        Joystick = FindObjectOfType<FloatingJoystick>();
        ActionButtonStatus(false);
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.IfPlayerMoving += IfPlayerMovingEventhandler;

        Joystick.actionButton.OnClickUp += OnActionButtonUpEventHandler;
        Joystick.actionButton.OnClickDown += OnActionButtonDownEventHandler;
    }

    protected override void OnDestroy()
    {
        Joystick.actionButton.OnClickUp -= OnActionButtonUpEventHandler;
        Joystick.actionButton.OnClickDown -= OnActionButtonDownEventHandler;
        playerMovement.IfPlayerMoving -= IfPlayerMovingEventhandler;
    }

    private void IfPlayerMovingEventhandler(bool flag)
    {
        isMoving = flag;
    }

    #endregion

    #region Action Buttons Functions
    private void ActionButtonStatus(bool flag, string text = "")
    {
        ActionButtonVisiblilty = flag;
        if (!flag)
        {
            return;
        }
        if (ActionString != text)
        {
            ActionString = text;
        }
    }
    #endregion

    #region Nearest Object Detection
    private void LateUpdate()
    {
        Collider2D[] groundOverlap = new Collider2D[4];
        Physics2D.OverlapCircleNonAlloc(transform.position, radius, groundOverlap, layerMask);
        nearestItem = GetClosestItem(groundOverlap);
        if (nearestItem != null)
        {
            NearestObject();
        }
        else
        {
            NoNearObject();
        }
        if (isActionButtonOnHold)
        {
            OnActionButtonDownEventHandler();
        }
    }

    private InteractiveItem GetClosestItem(Collider2D[] groundOverlap)
    {
        InteractiveItem bestTarget = null;
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
                bestTarget = groundOverlap[i].transform.GetComponent<InteractiveItem>();
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
    private void NearestObject()
    {
        SetPlayerToObjectDirection((transform.position - nearestItem.transform.position).normalized);
        itemType = nearestItem.interactiveItemType;
        UiDebugTextHandler.DebugText(itemType.ToString());
        if (itemType != InteractiveItemType.Dropped)
        {
            ShowHitLoction(nearestItem.transform.position);
            ActionButtonStatus(true, "Enter " + itemType);
        }
    }

    private void NoNearObject()
    {
        ActionButtonStatus(false);
        HideHitLocation();
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

    #region Action buttons pressed and hit complete

    private void OnActionButtonDownEventHandler()
    {
        if (isPlayerInAction)
        {
            return;
        }
        isActionButtonOnHold = true;
        switch (itemType)
        {
            case InteractiveItemType.None:
                break;
            case InteractiveItemType.Pickaxable:
                PickaxeAble pickaxeAble = nearestItem.GetComponent<PickaxeAble>();
                OnPickaxeAbleClicked.Invoke(pickaxeAble);
                break;
            case InteractiveItemType.Weaponable:
                break;
            case InteractiveItemType.Pickable:
                break;
            case InteractiveItemType.Axeable:
                break;
            case InteractiveItemType.Fishable:
                break;
            case InteractiveItemType.Searchable:
                break;
            case InteractiveItemType.OpenMenu:
                OnOpenMenuClicked.Invoke(nearestItem.openMenu);
                break;
            case InteractiveItemType.Enterance:
                OnEnteranceClicked.Invoke(nearestItem.enterance, nearestItem.minesLevel);
                break;
            case InteractiveItemType.Dropped:
                ActionButtonStatus(false);
                break;
            case InteractiveItemType.Building:
                break;
            case InteractiveItemType.Field:
                break;
            case InteractiveItemType.NPC:
                break;
            case InteractiveItemType.Livestock:
                break;
            default:
                ActionButtonStatus(false);
                break;
        }
    }

    private void OnActionButtonUpEventHandler()
    {
        isActionButtonOnHold = false;
    }
    #endregion
}

[System.Serializable]
public enum PlayerToObjectDirection
{
    None,
    Left,
    Right
}

[System.Serializable]
public enum InteractiveItemType
{
    None,
    Pickaxable,
    Weaponable,
    Pickable,
    Axeable,
    Fishable,
    Searchable, // on interaction get some random item
    OpenMenu,
    Enterance,
    Dropped,
    Building,
    Field,
    NPC,
    Livestock
}

[System.Serializable]
public enum EnteranceType
{
    None,
    Home,
    Mines,
    MinesLowerLevel,
    MinesSelectedLevel,
    Village
}

[System.Serializable]
public enum OpenMenuTypes
{
    None,
    MineLevelSelector,
    SomeXyz
}