using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Image dragImage;
    private Transform dragTransform;
    private Vector3 intialPosition;
    private Canvas mainCanvas;
    private RectTransform mainCanvasRect;
    private Vector2 pos;

    public virtual void Start()
    {
        if (dragTransform == null)
        {
            Debug.LogError("Assign dragImage in editor");
        }
        dragTransform = dragImage.transform;
        mainCanvas = UIMasterMenuManager.Instance.mainCanvas;
        mainCanvasRect = mainCanvas.GetComponent<RectTransform>();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvasRect, Input.mousePosition, mainCanvas.worldCamera, out pos);
        dragTransform.position = mainCanvas.transform.TransformPoint(pos);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        dragImage.raycastTarget = false;
        intialPosition = dragTransform.localPosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        dragImage.raycastTarget = true;
        dragTransform.localPosition = intialPosition;
    }
}