using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int itemID = 0;
    [SerializeField]
    private SpriteRenderer itemIcon;
    [SerializeField]
    private TextMeshPro itemAmountText;
    [SerializeField]
    private GameObject textBackground;
    private Vector3 intialPosition;
    private float zDistanceToCamera;
    private BoxCollider2D boxCollider2d;
    private Vector2 smallColliderSize = new Vector2(0.2f, 0.2f);

    public SpriteRenderer ItemIcon
    {
        get
        {
            return itemIcon;
        }
        set
        {
            ItemIcon = value;
        }
    }

    public string ItemAmountText
    {
        get
        {
            return itemAmountText.text;
        }
        set
        {
            itemAmountText.text = value;
        }
    }

    private void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, zDistanceToCamera));// + offsetToMouse;
        //MasterMenuManager.Instance.ChildCallingOnMouseDrag(itemID, transform.localPosition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        intialPosition = transform.localPosition;
        boxCollider2d.size = smallColliderSize;
        zDistanceToCamera = Mathf.Abs(intialPosition.z
            - Camera.main.transform.position.z);
        //MasterMenuManager.Instance.ChildCallingOnMouseDown(itemID, transform.localPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = intialPosition;
        boxCollider2d.size = Vector2.one;
        //MasterMenuManager.Instance.ChildCallingOnMouseUp(itemID);
    }
}