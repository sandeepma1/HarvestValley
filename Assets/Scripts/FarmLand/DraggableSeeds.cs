using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSeeds : MonoBehaviour
{
    public int seedID = 0;
    Vector3 intialPosition;

    void OnMouseDown()
    {
        intialPosition = transform.localPosition;
    }

    void OnMouseDrag()
    {
        //CropMenuManager.m_instance.ChildCallingOnMouseDrag (seedID);	
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, 10));
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnMouseUp()
    {
        transform.localPosition = intialPosition;
        //CropMenuManager.m_instance.ChildCallingOnMouseUp (seedID);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
