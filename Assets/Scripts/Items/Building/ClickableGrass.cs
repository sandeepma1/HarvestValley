using HarvestValley.Managers;
using UnityEngine;
using HarvestValley.IO;
using System;
using DG.Tweening;

public class ClickableGrass : ClickableBase
{
    public Grass grass;
    public Action OpenUiGrassMenu;
    public Action<int> ClickableGrassAddedItem;
    private Transform[] childrenTransform;
    private SpriteRenderer[] childrenSpriteRenderer;

    private void Start()
    {
        childrenTransform = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childrenTransform[i] = transform.GetChild(i);
        }

        childrenSpriteRenderer = GetComponentsInChildren<SpriteRenderer>();

        if (grass.itemId != -1)
        {
            SetChildGrassLeavesSprite(grass.itemId);
        }
    }

    private void SetChildGrassLeavesSprite(int itemId)
    {
        for (int i = 0; i < childrenTransform.Length; i++)
        {
            if (itemId == -1)
            {
                childrenSpriteRenderer[i].sprite = null;
            }
            else
            {
                string grassName = ItemDatabase.GetItemSlugById(itemId) + "_" + i;
                childrenTransform[i].localPosition = UnityEngine.Random.insideUnitCircle / 2.5f;
                childrenSpriteRenderer[i].sortingOrder = ((int)(childrenTransform[i].localPosition.y * -10)) + (int)transform.localPosition.y * -10;
                childrenSpriteRenderer[i].sprite = AtlasBank.Instance.GetSprite(grassName, AtlasType.Livestock);
            }
        }
    }

    public override void OnMouseTouchEnter()
    {
        if (GrassLandManager.Instance.isinPlantingMode && grass.itemId == -1)
        {
            grass.itemId = GrassLandManager.Instance.selectedItemIdInMenu;
            ClickableGrassAddedItem.Invoke(grass.itemId);
            SetChildGrassLeavesSprite(grass.itemId);
        }
    }

    public void RemovedGrass()
    {
        if (transform == null)  //TODO: what is this remove it
        {
            print("ClickableGrass still not instanciated");
            Start();
        }
        SetChildGrassLeavesSprite(-1);
        grass.itemId = -1;
    }

    public override void OnMouseTouchUp()
    {
        base.OnMouseTouchUp();
        if (grass.itemId == -1)
        {
            OpenUiGrassMenu.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (grass.itemId == -1)
        {
            return;
        }
        if (childrenTransform == null)
        {
            Start();
        }
        for (int i = 0; i < childrenTransform.Length; i++)
        {
            childrenTransform[i].DOShakeRotation(0.5f, new Vector3(0, 0, 25), 4);
        }
    }
}