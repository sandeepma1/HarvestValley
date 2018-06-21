using UnityEngine;

public class PickaxeAble : MonoBehaviour
{
    public int mineralId;
    public int outputId;

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
        MiningManager.Instance.SpwanItemAfterBreak(outputId, Vector2.zero);
        Destroy(this.gameObject);
    }
}