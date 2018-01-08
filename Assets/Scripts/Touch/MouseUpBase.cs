using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUpBase : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        if (!InputController.instance.isDragging)
        {
            TouchUp();
        }
    }

    public virtual void TouchUp()
    {

    }
}
