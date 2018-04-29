using UnityEngine;
using AStar_2D.Demo;

public class BuildingPointer : MouseUpBase
{
    [SerializeField]
    private Vector2 tilePosition;

    public override void OnMouseTouchUp()
    {
        TileManagerVillage.Instance.isClickedForBuilding = true;
        base.OnMouseTouchUp();
        TileManagerVillage.Instance.SetPlayerDestination(tilePosition);
    }
}