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
        GEM.isObjectDragging = true;
        HarvestMenuManager.Instance.isScytheSelected = true;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, 10));
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnMouseUp()
    {
        GEM.isObjectDragging = false;
        transform.localPosition = intialPosition;
        HarvestMenuManager.Instance.isScytheSelected = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
