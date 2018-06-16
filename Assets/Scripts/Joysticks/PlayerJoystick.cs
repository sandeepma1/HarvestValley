using System;
using UnityEngine;

public class PlayerJoystick : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private FloatingJoystick joystick;
    private Animator anim;
    private Vector3 moveVector;
    private Vector3 moveVectorTemp;
    private bool isMoving;
    private string actionTriggerColliderName;
    private string actionTagName;
    private Collider2D currentCollider2D;
    private bool inPlantingMode;

    private void Start()
    {
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FloatingJoystick>();
        joystick.OnJoystickClick += OnJoystickClickEventHandler;
        joystick.OnJoystickUp += OnJoystickUpEventHandler;
        joystick.OnActionButtonClick += OnActionButtonClickEventHandler;
        ActionButtonSetActive(false);
    }

    private void OnDestroy()
    {
        joystick.OnJoystickClick -= OnJoystickClickEventHandler;
        joystick.OnJoystickUp -= OnJoystickUpEventHandler;
        joystick.OnActionButtonClick -= OnActionButtonClickEventHandler;
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
            moveVector = (transform.right * joystick.Horizontal + transform.up * joystick.Vertical).normalized;
            transform.Translate(moveVector * moveSpeed * Time.deltaTime);
            anim.SetFloat("Player_Forward", joystick.Vertical);
            anim.SetFloat("Player_Left", joystick.Horizontal);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void ActionButtonSetActive(bool flag)
    {
        joystick.actionButton.gameObject.SetActive(flag);
    }

    private void ActionButtonText(string text)
    {
        joystick.actionText.text = text;
    }

    private void OnActionButtonClickEventHandler()
    {
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

    private void SetField()
    {
        ClickableField clickableField = currentCollider2D.GetComponent<ClickableField>();
        clickableField.SomeSeedPlanted();
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

    private void OnTriggerStay2D(Collider2D collision)
    {
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
}
