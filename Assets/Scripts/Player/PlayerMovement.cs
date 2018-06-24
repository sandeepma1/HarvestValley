using System;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public event Action<bool> IfPlayerMoving;
    private FloatingJoystick Joystick;
    private Animator anim;
    private Vector3 moveVector;
    private bool isMoving;
    [SerializeField]
    private float moveSpeed = 3.75f;
    [SerializeField]
    private Transform characterRig;

    #region Unity Default
    private void Start()
    {
        anim = GetComponent<Animator>();
        Joystick = FindObjectOfType<FloatingJoystick>();
        Joystick.OnJoystickClick += OnJoystickClickEventHandler;
        Joystick.OnJoystickUp += OnJoystickUpEventHandler;
    }

    protected override void OnDestroy()
    {
        Joystick.OnJoystickClick -= OnJoystickClickEventHandler;
        Joystick.OnJoystickUp -= OnJoystickUpEventHandler;
    }
    #endregion

    #region Playermovement
    private void OnJoystickClickEventHandler()
    {
        isMoving = true;
        if (IfPlayerMoving != null)
        {
            IfPlayerMoving.Invoke(isMoving);
        }
    }

    private void OnJoystickUpEventHandler()
    {
        isMoving = false;
        if (IfPlayerMoving != null)
        {
            IfPlayerMoving.Invoke(isMoving);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            if (PlayerController.Instance.isPlayerInAction)
            {
                return;
            }

            moveVector = (transform.right * Joystick.Horizontal + transform.up * Joystick.Vertical).normalized;
            if (Mathf.Abs(moveVector.x) == 0)
            {
                return;
            }
            anim.SetBool("isMoving", true);
            transform.Translate(moveVector * moveSpeed * Time.deltaTime);
            if (moveVector.x > 0)
            {
                TurnPlayerRight();
            }
            else
            {
                TurnPlayerLeft();
            }
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void TurnPlayerRight()
    {
        characterRig.eulerAngles = new Vector3(0, 180, 0);
    }

    private void TurnPlayerLeft()
    {
        characterRig.eulerAngles = Vector3.zero;
    }

    public void PlayerToolAction(bool flag)
    {
        if (anim != null)
        {
            switch (PlayerController.Instance.playerToObjectDirection)
            {
                case PlayerToObjectDirection.None:
                    break;
                case PlayerToObjectDirection.Left:
                    TurnPlayerLeft();
                    break;
                case PlayerToObjectDirection.Right:
                    TurnPlayerRight();
                    break;
                default:
                    break;
            }
            anim.SetBool("isPickaxe", flag);
        }
    }
    #endregion
}