using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingJoystick : Joystick
{
    public event Action OnJoystickClick;
    public event Action OnJoystickUp;
    Vector2 joystickCenter = Vector2.zero;
    [SerializeField]
    internal HitAction actionButton;
    [SerializeField]
    internal HitAction secondaryButton;
    [SerializeField]
    internal TextMeshProUGUI actionText;
    [SerializeField]
    internal TextMeshProUGUI secondaryText;

    private void Start()
    {
        background.gameObject.SetActive(false);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (PlayerController.Instance.isPlayerInAction)
        {
            return;
        }
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