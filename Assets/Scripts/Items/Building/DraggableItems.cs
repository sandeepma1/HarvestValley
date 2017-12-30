using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int itemID = 0;
    Vector3 intialPosition;
    float zDistanceToCamera;
    //Vector3 offsetToMouse;

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, zDistanceToCamera));// + offsetToMouse;
        MasterMenuManager.Instance.ChildCallingOnMouseDrag(itemID, transform.localPosition);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        intialPosition = transform.localPosition;
        zDistanceToCamera = Mathf.Abs(intialPosition.z
            - Camera.main.transform.position.z);
        //offsetToMouse = intialPosition - Camera.main.ScreenToWorldPoint(
        //new Vector3(Input.mousePosition.x,
        //Input.mousePosition.y, zDistanceToCamera));
        MasterMenuManager.Instance.ChildCallingOnMouseDown(itemID, transform.localPosition);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // offsetToMouse = Vector3.zero;
        transform.localPosition = intialPosition;
        MasterMenuManager.Instance.ChildCallingOnMouseUp(itemID);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
