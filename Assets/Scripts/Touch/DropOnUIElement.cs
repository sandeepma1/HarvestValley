using UnityEngine;
using UnityEngine.EventSystems;
using HarvestValley.Managers;

namespace HarvestValley.Ui
{
    public class DropOnUIElement : MonoBehaviour, IDropHandler
    {
        private UiDraggableItem item;

        public void OnDrop(PointerEventData eventData)
        {
            item = eventData.pointerDrag.gameObject.GetComponent<UiDraggableItem>();

            if (item == null || !item.isItemUnlocked) { return; }

            UiBuildingMenu.Instance.ItemDroppedInZone(item.itemID);
        }
    }
}