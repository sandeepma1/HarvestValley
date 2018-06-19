using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<bool> IfPlayerMoving;
    private FloatingJoystick Joystick;
    private Animator anim;
    private Vector3 moveVector;
    private bool isMoving;
    [SerializeField]
    private float moveSpeed = 3.75f;

    #region Unity Default
    private void Start()
    {
        anim = GetComponent<Animator>();
        Joystick = FindObjectOfType<FloatingJoystick>();
        Joystick.OnJoystickClick += OnJoystickClickEventHandler;
        Joystick.OnJoystickUp += OnJoystickUpEventHandler;
    }

    private void OnDestroy()
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
}