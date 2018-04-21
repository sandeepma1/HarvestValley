using UnityEngine.EventSystems;
using UnityEngine;

public class Dragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject DraggedInstance;

    Vector3 startPosition;
    Vector3 offsetToMouse;
    float zDistanceToCamera;

    #region Interface Implementations
    public void OnBeginDrag(PointerEventData eventData)
    {
        //.isObjectDragging = true;
        //DraggedInstance = gameObject;
        startPosition = transform.position;
        zDistanceToCamera = Mathf.Abs(startPosition.z - Camera.main.transform.position.z);

        offsetToMouse = startPosition - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistanceToCamera)
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistanceToCamera)
            ) + offsetToMouse;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //GEM.isObjectDragging = false;
        offsetToMouse = Vector3.zero;
        transform.position = startPosition;
    }
    #endregion
}