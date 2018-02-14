using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUIHarvesting : DraggableUIBase
{
    public static DraggableUIHarvesting Instance = null;

    public override void OnEndDrag(PointerEventData eventData)
    {

        base.OnEndDrag(eventData);
    }
}
