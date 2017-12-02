using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableHarvesting : MonoBehaviour
{
    Vector3 intialPosition;

    void OnMouseDown()
    {
        intialPosition = transform.localPosition;
    }

    void OnMouseDrag()
    {
        GEM.isDragging = true;
        HarvestMenuManager.Instance.isScytheSelected = true;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, 10));
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnMouseUp()
    {
        GEM.isDragging = false;
        transform.localPosition = intialPosition;
        HarvestMenuManager.Instance.isScytheSelected = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
