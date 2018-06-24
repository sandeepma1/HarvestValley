using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitAction : UIBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnClickDown;
    public event Action OnClickUp;

    protected override void OnDisable()
    {
        if (OnClickUp != null)
        {
            OnClickUp.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClickDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnClickUp.Invoke();
    }
}