using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

/// <summary>
/// Thanks for https://forum.unity.com/threads/nested-scrollrect.268551/
/// </summary>
namespace HarvestValley.Ui
{
    public class UiDraggableItem : ScrollRect, IPointerDownHandler
    {
        [SerializeField]
        private TextMeshProUGUI itemNameText;

        [SerializeField]
        internal Image itemImage;

        internal int itemID;
        internal string itemName;
        internal bool isItemUnlocked;

        public int selectedItemID;
        public Action<int> ItemClickedDragged;
        #region Drag Variables
        private bool routeToParent = false;
        private Transform imageImageTransform;
        private Vector3 initialPosition;
        private Vector2 pos;
        private UiDraggableItemHelper helper;
        #endregion

        protected override void Start()
        {
            helper = GetComponent<UiDraggableItemHelper>();
            itemNameText = helper.itemNameText;
            imageImageTransform = helper.itemImage.GetComponent<Transform>();
            itemImage = imageImageTransform.GetComponent<Image>();
            selectedItemID = -1;
        }

        public void ItemUnlocked()
        {
            isItemUnlocked = true;
            itemImage.color = ColorConstants.NormalUiItem;
            itemNameText.text = itemName;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ItemClickedDragged.Invoke(itemID);
        }

        private void _OnBeginDrag()
        {
            itemImage.raycastTarget = false;
            initialPosition = imageImageTransform.position;
            selectedItemID = itemID;
        }

        private void _OnDrag()
        {
            if (Input.touchCount > 1) { return; }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(UiBuildingMenu.Instance.mainCanvas.transform as RectTransform,
                Input.mousePosition, UiBuildingMenu.Instance.mainCanvas.worldCamera, out pos);
            imageImageTransform.position = UiBuildingMenu.Instance.mainCanvas.transform.TransformPoint(pos);
        }

        private void _OnEndDrag()
        {
            itemImage.raycastTarget = true;
            imageImageTransform.position = initialPosition;
            //SeedListMenu.Instance.OnDragComplete(selectedItemID);
            selectedItemID = -1;
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
            }
            else
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
}