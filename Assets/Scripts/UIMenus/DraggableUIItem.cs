using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Thanks for https://forum.unity.com/threads/nested-scrollrect.268551/
/// </summary>
public class DraggableUIItem : ScrollRect
{
    public int itemID;

    private bool routeToParent = false;
    private Transform imageTransform;
    private Canvas myCanvas;
    private Vector3 initialPosition;
    private Vector2 pos;
    private DraggableUIItemHelper helper;

    public TextMeshProUGUI itemAmountText;
    public Image itemImage;
    public Action<int> SelectedItemID;

    protected override void Start()
    {
        helper = GetComponent<DraggableUIItemHelper>();
        itemAmountText = helper.itemCostText;
        imageTransform = helper.itemImage.GetComponent<Transform>();
        myCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        if (SelectedItemID != null)
        {
            SelectedItemID.Invoke(-1);
        }
    }

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
    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {

        if (routeToParent)
        {
            DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
        } else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            imageTransform.position = myCanvas.transform.TransformPoint(pos);
            base.OnDrag(eventData);
        }
    }

    /// <summary>
    /// Begin drag event
    /// </summary>
    public override void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
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
            initialPosition = imageTransform.position;
            if (SelectedItemID != null)
            {
                SelectedItemID.Invoke(itemID);
            }
            base.OnBeginDrag(eventData);
        }
    }

    /// <summary>
    /// End drag event
    /// </summary>
    public override void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (routeToParent)
            DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(eventData); });
        else
        {
            imageTransform.position = initialPosition;
            if (SelectedItemID != null)
            {
                SelectedItemID.Invoke(-1);
            }
            base.OnEndDrag(eventData);
        }
        routeToParent = false;
    }
}