using UnityEngine;

public class PickaxeAble : MonoBehaviour
{
    public int mineralId;
    public int outputId;
    public bool hasLadder;

    private int hitspoints;
    public int HitPoints
    {
        get
        {
            return hitspoints;
        }
        set
        {
            hitspoints = value;
            if (hitspoints <= 0)
            {
                ItemBroken();
            }
        }
    }

    private void ItemBroken()
    {
        if (hasLadder)
        {
            ItemDropperManager.Instance.SpawnLadder(transform.position);
        }
        else
        {
            ItemDropperManager.Instance.DropItem(outputId, transform.position);
        }
        Destroy(this.gameObject);
    }
}