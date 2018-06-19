using UnityEngine;
using HarvestValley.IO;
using System;

public class PlayerController : Singleton<PlayerController>
{
    private FloatingJoystick Joystick;
    public event Action<int> OnPickaxeClicked;
    public event Action<string> OnEnteranceClicked;
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

    #region Unity Default
    private void Start()
    {
        hitBox = transform.GetChild(0);
        Joystick = FindObjectOfType<FloatingJoystick>();
        Joystick.OnActionButtonClick += OnActionButtonClickEventHandler;
        Joystick.OnActionButtonClick += OnSecondaryButtonClickEventHandler;
        ActionButtonSetActive(false);
        SecondaryButtonSetActive(false);
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.IfPlayerMoving += IfPlayerMovingEventhandler;
    }

    private void IfPlayerMovingEventhandler(bool flag)
    {
        isMoving = flag;
    }

    protected override void OnDestroy()
    {
        Joystick.OnActionButtonClick -= OnActionButtonClickEventHandler;
        Joystick.OnActionButtonClick -= OnSecondaryButtonClickEventHandler;
        playerMovement.IfPlayerMoving -= IfPlayerMovingEventhandler;
    }
    #endregion

    #region Action Secondary Buttons Functions
    private void ActionButtonSetActive(bool flag)
    {
        Joystick.ActionButtonVisiblilty = flag;
    }

    private void SecondaryButtonSetActive(bool flag)
    {
        Joystick.SecondaryButtonVisiblilty = flag;
    }

    private void ActionButtonText(string text)
    {
        Joystick.ActionString = text;
    }

    private void SecondaryButtonText(string text)
    {
        Joystick.SecondaryString = text;
    }
    #endregion

    #region Nearest Object Detection
    private void LateUpdate()
    {
        if (isMoving)
        {
            Collider2D[] groundOverlap = new Collider2D[4];
            Physics2D.OverlapCircleNonAlloc(transform.position, radius, groundOverlap, layerMask);
            if (GetClosestEnemy(groundOverlap) != null)
            {
                nearestObject = GetClosestEnemy(groundOverlap);
                hitBox.transform.position = nearestObject.position;
                NearestObject(nearestObject);
            }
            else
            {
                nearestObject = null;
                hitBox.transform.position = Vector3.zero;
                NoNearObject();
            }
        }
    }

    Transform GetClosestEnemy(Collider2D[] groundOverlap)
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
    #endregion

    #region Display Data on Action Secondary Buttons
    private void NearestObject(Transform collision)
    {
        actionTagName = collision.tag;
        actionTriggerColliderName = collision.name;
        UiDebugTextHandler.DebugText(actionTriggerColliderName);
        ActionButtonSetActive(true);
        switch (actionTagName)
        {
            case "Enterence":
                ActionButtonText("Enter " + actionTriggerColliderName);
                break;
            case "Pickaxe":
                ActionButtonText("Pickaxe");
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
    }
    #endregion

    #region Action Secondary buttons pressed
    private void OnActionButtonClickEventHandler()
    {
        ActionButtonSetActive(false);
        switch (actionTagName)
        {
            case "Enterence":
                OnEnteranceClicked.Invoke(actionTriggerColliderName);
                break;
            case "Pickaxe":
                int itemId = int.Parse(nearestObject.name);
                OnPickaxeClicked.Invoke(itemId);
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