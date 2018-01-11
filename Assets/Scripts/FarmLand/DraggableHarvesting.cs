using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableHarvesting : DraggableBase
{
    public static DraggableHarvesting Instance = null;
    private bool isHarvestComplete;

    private void Awake()
    {
        Instance = this;
    }

    public void OnHarvestComplete()
    {
        isHarvestComplete = true;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isHarvestComplete)
        {
            UIMasterMenuManager.Instance.OnHarvestComplete();
            isHarvestComplete = false;
        }
        base.OnEndDrag(eventData);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}
