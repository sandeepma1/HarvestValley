using HarvestValley.Ui;
using UnityEngine;
using HarvestValley.IO;

public class LivestockOutput : MouseUpBase
{
    private int itemId;
    private SpriteRenderer itemImage;

    public void CreateOutput(int _itemId)
    {
        itemId = _itemId;
        itemImage = GetComponent<SpriteRenderer>();
        itemImage.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemSlugById(itemId), AtlasType.GUI);
        print(ItemDatabase.GetItemSlugById(itemId));
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        UiInventoryMenu.Instance.UpdateItems(itemId, 1);
        Destroy(gameObject);
    }
}