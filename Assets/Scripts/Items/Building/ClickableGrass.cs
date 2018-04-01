using HarvestValley.Managers;
using UnityEngine;
using HarvestValley.IO;
using System;

public class ClickableGrass : ClickableBase
{
    public Grass grass;
    private SpriteRenderer spriteRenderer;
    public Action<int> ClickableGrasssClicked;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (grass.itemId != -1) // 
        {
            spriteRenderer.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemNameById(grass.itemId), AtlasType.Lifestock);
        }
    }

    private void OnMouseEnter()
    {
        if (GrassLandManager.Instance.isinPlantingMode && grass.itemId == -1)
        {
            grass.itemId = GrassLandManager.Instance.selectedItemIdInMenu;
            spriteRenderer.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemNameById(grass.itemId), AtlasType.Lifestock);
        }
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        ClickableGrasssClicked.Invoke(grass.itemId);
    }
}
