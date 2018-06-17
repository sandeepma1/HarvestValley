using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public event Action OnJoystickClick;
    public event Action OnActionButtonClick;
    public event Action OnSecondaryButtonClick;
    public event Action OnJoystickUp;
    Vector2 joystickCenter = Vector2.zero;

    private void Start()
    {
        actionButton.onClick.AddListener(OnActionButtonClickEventHandler);
        secondaryButton.onClick.AddListener(OnSecondaryButtonClickEventHandler);
        background.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        actionButton.onClick.RemoveListener(OnActionButtonClickEventHandler);
        secondaryButton.onClick.RemoveListener(OnSecondaryButtonClickEventHandler);
    }

    private void OnActionButtonClickEventHandler()
    {
        if (OnActionButtonClick != null)
        {
            OnActionButtonClick.Invoke();
        }
    }

    private void OnSecondaryButtonClickEventHandler()
    {
        if (OnSecondaryButtonClick != null)
        {
            OnSecondaryButtonClick.Invoke();
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        //OnJoystickDrag.Invoke();
        Vector2 direction = eventData.position - joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (OnJoystickClick != null)
        {
            OnJoystickClick.Invoke();
        }
        background.gameObject.SetActive(true);
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        joystickCenter = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (OnJoystickUp != null)
        {
            OnJoystickUp.Invoke();
        }
        background.gameObject.SetActive(false);
        inputVector = Vector2.zero;
    }
}