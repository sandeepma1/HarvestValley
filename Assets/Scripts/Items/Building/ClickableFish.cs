using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarvestValley.IO;
using System;

public class ClickableFish : ClickableBase
{
    public Fishes fish;
    public Action<int> OnFishClicked;
    private SpriteRenderer fishSprite;
    private DateTime nextSpawnDateTime;

    private void Start()
    {
        Item fishItem = ItemDatabase.GetItemById(fish.itemId);
        gameObject.name = "Fish" + fish.itemId;
        fishSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        fishSprite.sprite = AtlasBank.Instance.GetSprite(fishItem.slug, AtlasType.GUI);
        nextSpawnDateTime = DateTime.Parse(fish.nextSpawnDateTime);
    }


    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        OnFishClicked.Invoke(fish.itemId);
    }
}