using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItems : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public int itemID = 0;
    Vector3 intialPosition;

    public void OnDrag(PointerEventData eventData)
    {
        GEM.isObjectDragging = true;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 30, 10));
        MasterMenuManager.Instance.ChildCallingOnMouseDrag(itemID, transform.localPosition);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        intialPosition = transform.localPosition;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 30, 10));
        MasterMenuManager.Instance.ChildCallingOnMouseDown(itemID, transform.localPosition);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GEM.isObjectDragging = false;
        transform.localPosition = intialPosition;
        MasterMenuManager.Instance.ChildCallingOnMouseUp(itemID);
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
