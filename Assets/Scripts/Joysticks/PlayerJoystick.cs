using HarvestValley.Managers;
using System;
using UnityEngine;

public class PlayerJoystick : Singleton<PlayerJoystick>
{
    [SerializeField]
    private float moveSpeed;
    private FloatingJoystick Joystick;
    private Animator anim;
    private Vector3 moveVector;
    private Vector3 moveVectorTemp;
    private bool isMoving;
    private string actionTriggerColliderName;
    private string actionTagName;
    private Collider2D currentCollider2D;
    private bool inPlantingMode;
    private int selectedID = -1;

    private void Start()
    {
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
    // ----------------

    private void OnActionButtonClickEventHandler()
    {
        if (inPlantingMode)
        {
            FieldManager.Instance.StopPlantingMode();     //Cancel planting.
            inPlantingMode = false;
            return;
        }
        ActionButtonSetActive(false);
        switch (actionTagName)
        {
            case "Enterence":
                SetEnterence(actionTriggerColliderName);
                break;
            case "Field":
                SetField();
                break;
            default:
                break;
        }
    }

    private void OnSecondaryButtonClickEventHandler()
    {
        if (inPlantingMode)
        {
            SetField();     //Select seed menu open.
        }
        SecondaryButtonSetActive(false);
    }

    #region All Trigger Collision
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (inPlantingMode)
        {
            ActionButtonSetActive(true);
            ActionButtonText("Cancel Planting");
            return;
        }
        if (currentCollider2D == null)
        {
            currentCollider2D = collision;
        }
        actionTagName = collision.tag;
        actionTriggerColliderName = collision.name;
        ActionButtonSetActive(true);
        switch (actionTagName)
        {
            case "Enterence":
                ActionButtonText("Enter " + actionTriggerColliderName);
                break;
            case "Field":
                ActionButtonText("Plant");
                break;
            default:
                ActionButtonSetActive(false);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (inPlantingMode)
        {
            return;
        }
        ActionButtonSetActive(false);
        currentCollider2D = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!inPlantingMode)
        {
            return;
        }

    }
    #endregion

    #region Other Functions
    public void StartPlantingMode(int itemID = -1)
    {
        selectedID = itemID;
        inPlantingMode = true;
        SecondaryButtonSetActive(true);
        SecondaryButtonText("Select Seed");
    }

    public void StopPlantingMode()
    {
        selectedID = -1;
    }

    private void SetField()
    {
        ClickableField clickableField = currentCollider2D.GetComponent<ClickableField>();
        clickableField.ShowCropMenu();
        inPlantingMode = true;
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
