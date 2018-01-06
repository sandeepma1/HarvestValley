using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

/// <summary>
/// Thanks for https://forum.unity.com/threads/nested-scrollrect.268551/
/// </summary>
public class DraggableUIItem : ScrollRect
{
    public int itemID;
    private bool routeToParent = false;
    private Transform imageImageTransform;
    private Canvas myCanvas;
    private Vector3 initialPosition;
    private Vector2 pos;
    private DraggableUIItemHelper helper;
    public TextMeshProUGUI itemCostText;
    public TextMeshProUGUI itemNameText;
    public Image itemImage;
    public int selectedItemID;

    protected override void Awake()
    {
        helper = GetComponent<DraggableUIItemHelper>();
        itemCostText = helper.itemCostText;
        itemNameText = helper.itemNameText;
        imageImageTransform = helper.itemImage.GetComponent<Transform>();
        itemImage = imageImageTransform.GetComponent<Image>();
        myCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        selectedItemID = -1;
    }

    private void _OnBeginDrag()
    {
        itemImage.raycastTarget = false;
        initialPosition = imageImageTransform.position;
        selectedItemID = itemID;
    }

    private void _OnDrag()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        imageImageTransform.position = myCanvas.transform.TransformPoint(pos);
    }

    private void _OnEndDrag()
    {
        itemImage.raycastTarget = true;
        imageImageTransform.position = initialPosition;
        UIMasterMenuManager.Instance.OnDragComplete(selectedItemID);
        selectedItemID = -1;
        print("endDrag done");
    }

    #region ScrollRect Stuff DO NOT EDIT
    /// <summary>
    /// Do action for all parents
    /// </summary>
    private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
    {
        Transform parent = transform.parent;
        while (parent != null)
        {
            foreach (var component in parent.GetComponents<Component>())
            {
                if (component is T)
                    action((T)(IEventSystemHandler)component);
            }
            parent = parent.parent;
        }
    }

    /// <summary>
    /// Always route initialize potential drag event to parents
    /// </summary>
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        DoForParents<IInitializePotentialDragHandler>((parent) => { parent.OnInitializePotentialDrag(eventData); });
        base.OnInitializePotentialDrag(eventData);
    }

    /// <summary>
    /// Drag event
    /// </summary>
    public override void OnDrag(PointerEventData eventData)
    {
        if (routeToParent)
        {
            DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
        } else
        {
            _OnDrag();
            base.OnDrag(eventData);
        }
    }

    /// <summary>
    /// Begin drag event
    /// </summary>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
            routeToParent = true;
        else if (!vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
            routeToParent = true;
        else
            routeToParent = false;

        if (routeToParent)
            DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(eventData); });
        else
        {
            _OnBeginDrag();
            base.OnBeginDrag(eventData);
        }
    }

    /// <summary>
    /// End drag event
    /// </summary>
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (routeToParent)
            DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(eventData); });
        else
        {
            _OnEndDrag();
            base.OnEndDrag(eventData);
        }
        routeToParent = false;
    }
    #endregion
}