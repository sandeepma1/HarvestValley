using HarvestValley.Ui;
using UnityEngine;
using HarvestValley.IO;
using System;

public class LivestockOutput : MouseUpBase
{
    private int itemId;
    private SpriteRenderer itemImage;
    public Action ItemCollected;

    public void CreateOutput(int _itemId)
    {
        itemId = _itemId;
        itemImage = GetComponent<SpriteRenderer>();
        itemImage.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemSlugById(itemId), AtlasType.GUI);
        itemImage.sortingOrder = (int)(transform.localPosition.y * -10) + 11;
        print(ItemDatabase.GetItemSlugById(itemId));
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        UiInventoryMenu.Instance.UpdateItems(itemId, 1);
        ItemCollected.Invoke();
        Destroy(gameObject);
    }
}