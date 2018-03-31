using HarvestValley.Managers;
using UnityEngine;
using HarvestValley.IO;
using HarvestValley.Ui;

public class ClickableGrass : MonoBehaviour
{
    public Grass grass;
    private SpriteRenderer spriteRenderer;
    private Sprite grassSprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        grassSprite = AtlasBank.Instance.GetSprite("basicgrass", AtlasType.Lifestock);
        if (grass.itemId != -1) // 
        {
            spriteRenderer.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemNameById(grass.itemId), AtlasType.Lifestock);
        }
    }

    private void OnMouseEnter()
    {
        if (GrassLandManager.Instance.isPlantingMode && grass.itemId == -1)
        {
            grass.itemId = 4;
            spriteRenderer.sprite = grassSprite;
        }
    }

    //private void OnMouseDown()
    //{
    //    GrassLandManager.Instance.isPlantingMode = !GrassLandManager.Instance.isPlantingMode;

    //    if (GrassLandManager.Instance.isPlantingMode)
    //    {
    //        InputController.Instance.SetDragSwipe(false);
    //        UiGrassListMenu.Instance.StartPlantingMode(grass.itemId);
    //    }
    //    else
    //    {
    //        InputController.Instance.SetDragSwipe(true);
    //        UiGrassListMenu.Instance.StopPlantingMode();
    //        GrassLandManager.Instance.ChangedSometingSaveGrass();
    //    }
    //}
}
