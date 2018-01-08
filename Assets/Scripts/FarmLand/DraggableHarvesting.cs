using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableHarvesting : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static DraggableHarvesting Instance = null;
    private Transform scytheImageTransform;
    private Image scytheImage;

    private Vector3 intialPosition;
    private Canvas mainCanvas;
    private Vector2 pos;

    private bool isHarvestComplete;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        scytheImageTransform = transform;
        scytheImage = GetComponent<Image>();
        mainCanvas = UIMasterMenuManager.Instance.mainCanvas;
    }

    public void OnHarvestComplete()
    {
        isHarvestComplete = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, Input.mousePosition, mainCanvas.worldCamera, out pos);
        scytheImageTransform.position = mainCanvas.transform.TransformPoint(pos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        scytheImage.raycastTarget = false;
        intialPosition = scytheImageTransform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isHarvestComplete)
        {
            UIMasterMenuManager.Instance.OnHarvestComplete();
            isHarvestComplete = false;
        }
        scytheImage.raycastTarget = true;
        scytheImageTransform.position = intialPosition;
        print("drag");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}
