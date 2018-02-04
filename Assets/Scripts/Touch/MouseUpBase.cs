﻿using UnityEngine;
using UnityEngine.EventSystems;

public class MouseUpBase : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        var touch = InputHelper.GetTouches();
        if (touch.Count > 0)
        {
            Touch t = touch[0];

            if (Application.isEditor)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
            }
            else
            {
                if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
                {
                    return;
                }
            }

            OnMouseTouchUp();
        }
    }

    public virtual void OnMouseTouchUp()
    {

    }
}