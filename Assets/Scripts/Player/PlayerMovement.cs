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
            anim.SetBool("isMoving", true);
            moveVector = (transform.right * Joystick.Horizontal + transform.up * Joystick.Vertical).normalized;
            //UiDebugTextHandler.DebugText(Joystick.Vertical + " " + Joystick.Horizontal + " " + moveVector);
            transform.Translate(moveVector * moveSpeed * Time.deltaTime);
            if (moveVector.x > 0)
            {
                FlipPlayer();
            }
            else
            {
                NormalPlayer();
            }
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void FlipPlayer()
    {
        characterRig.eulerAngles = new Vector3(0, 180, 0);
    }

    private void NormalPlayer()
    {
        characterRig.eulerAngles = Vector3.zero;
    }

    public void PlayerPickaxeAction(bool flag)
    {
        if (anim != null)
        {
            anim.SetBool("isPickaxe", flag);
        }
    }
    #endregion
}