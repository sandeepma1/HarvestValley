using UnityEngine;

public class ItemDropperManager : Singleton<ItemDropperManager>
{
    [SerializeField]
    private DroppedItem droppedItemPrefab;
    [SerializeField]
    private LadderSpawner ladderSpawnerPrefab;

    public void DropItem(int itemId, Vector3 location)
    {
        DroppedItem item = Instantiate(droppedItemPrefab, location, Quaternion.identity, this.transform);
        item.itemId = itemId;
    }

    public void SpawnLadder(Vector3 location)
    {
        LadderSpawner item = Instantiate(ladderSpawnerPrefab, location, Quaternion.identity, this.transform);
    }
}