using HarvestValley.Managers;
using System;
using UnityEngine;
using HarvestValley.IO;

public class PlayerJoystick : Singleton<PlayerJoystick>
{
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float radius = 1;
    [SerializeField]
    private float moveSpeed;

    private FloatingJoystick Joystick;
    private Animator anim;
    private Vector3 moveVector;
    private bool isMoving;
    public string actionTriggerColliderName;
    public string actionTagName;
    private Collider2D currentCollider2D;
    private Transform hitBox;
    private Transform nearestObject;

    #region Unity Default
    private void Start()
    {
        hitBox = transform.GetChild(0);
        anim = GetComponent<Animator>();
        Joystick = FindObjectOfType<FloatingJoystick>();
        Joystick.OnJoystickClick += OnJoystickClickEventHandler;
        Joystick.OnJoystickUp += OnJoystickUpEventHandler;
        Joystick.OnActionButtonClick += OnActionButtonClickEventHandler;
        Joystick.OnActionButtonClick += OnSecondaryButtonClickEventHandler;
        ActionButtonSetActive(false);
        SecondaryButtonSetActive(false);
    }

    protected override void OnDestroy()
    {
        Joystick.OnJoystickClick -= OnJoystickClickEventHandler;
        Joystick.OnJoystickUp -= OnJoystickUpEventHandler;
        Joystick.OnActionButtonClick -= OnActionButtonClickEventHandler;
        Joystick.OnActionButtonClick -= OnSecondaryButtonClickEventHandler;
    }
    #endregion

    #region Playermovement
    private void OnJoystickClickEventHandler()
    {
        isMoving = true;
    }

    private void OnJoystickUpEventHandler()
    {
        isMoving = false;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            anim.SetBool("isMoving", true);
            moveVector = (transform.right * Joystick.Horizontal + transform.up * Joystick.Vertical).normalized;
            transform.Translate(moveVector * moveSpeed * Time.deltaTime);
            anim.SetFloat("Player_Forward", Joystick.Vertical);
            anim.SetFloat("Player_Left", Joystick.Horizontal);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
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
                SetEnterence(actionTriggerColliderName);
                break;
            case "Pickaxe":
                UsePickaxe();
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

    #region Other Functions
    private void UsePickaxe()
    {
        int itemId = int.Parse(nearestObject.name);
        int hit = MineralsDatabase.GetMineralsInfoById(itemId).hits;
        int output = MineralsDatabase.GetMineralsInfoById(itemId).outputId;
        print(itemId + " " + hit + " " + output);
    }

    private void SetEnterence(string enteranceName)
    {
        switch (enteranceName)
        {
            case "Mines":
                SceneChanger.Instance.LoadScene("Mines");
                break;
            default:
                break;
        }
    }
    #endregion
}
