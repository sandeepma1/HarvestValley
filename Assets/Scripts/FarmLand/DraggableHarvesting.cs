using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableHarvesting : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 intialPosition;

    //void OnMouseDown()
    //{
    //    intialPosition = transform.localPosition;
    //}

    //void OnMouseDrag()
    //{
    //    GEM.isObjectDragging = true;
    //    HarvestMenuManager.Instance.isScytheSelected = true;
    //    transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, 10));
    //    GetComponent<BoxCollider2D>().enabled = false;
    //}

    //void OnMouseUp()
    //{
    //    GEM.isObjectDragging = false;
    //    transform.localPosition = intialPosition;
    //    HarvestMenuManager.Instance.isScytheSelected = false;
    //    GetComponent<BoxCollider2D>().enabled = true;
    //}

    float zDistanceToCamera;
    //Vector3 offsetToMouse;

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        GEM.isObjectDragging = true;
        HarvestMenuManager.Instance.isScytheSelected = true;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, 10));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        intialPosition = transform.localPosition;
        zDistanceToCamera = Mathf.Abs(intialPosition.z
            - Camera.main.transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GEM.isObjectDragging = false;
        transform.localPosition = intialPosition;
        HarvestMenuManager.Instance.isScytheSelected = false;
    }
}
