using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableHarvesting : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 intialPosition;
    float zDistanceToCamera;
    private BoxCollider2D boxCollider2d;
    private Vector2 smallColliderSize = new Vector2(0.2f, 0.2f);

    private void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        HarvestMenuManager.Instance.isScytheSelected = true;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, 10));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        boxCollider2d.size = smallColliderSize;
        intialPosition = transform.localPosition;
        zDistanceToCamera = Mathf.Abs(intialPosition.z
            - Camera.main.transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = intialPosition;
        boxCollider2d.size = Vector2.one;
        HarvestMenuManager.Instance.isScytheSelected = false;
    }
}
