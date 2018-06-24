using UnityEngine;
using System.Collections.Generic;
using CreativeSpore.SuperTilemapEditor;
using HarvestValley.IO;

public class MiningManager : Singleton<MiningManager>
{
    [SerializeField]
    private Tilemap tileMap;
    private List<MineItems> propsCells = new List<MineItems>();
    private float totalCells;
    private GameObject player;
    private PickaxeAble mineralPrefab;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mineralPrefab = Resources.Load("mineral", typeof(PickaxeAble)) as PickaxeAble;
        tileMap.IsVisible = false;
        ReadPropsCells();
        GenerateProps();
    }

    private void ReadPropsCells()
    {
        totalCells = tileMap.GridHeight * tileMap.GridWidth;
        for (int i = 0; i < tileMap.GridHeight; i++)
        {
            for (int j = 0; j < tileMap.GridWidth; j++)
            {
                if (tileMap.GetTileData(i, j) <= totalCells)
                {
                    propsCells.Add(new MineItems(tileMap.GetTileData(i, j), i, j));
                }
            }
        }
    }

    private void GenerateProps()
    {
        for (int i = 0; i < propsCells.Count; i++)
        {
            if (propsCells[i].itemId == 0) // Spawn Exit Mines collider
            {
                SpawnExitMines(propsCells[i]);
                continue;
            }
            if (propsCells[i].itemId == 1) // Spawn Player
            {
                SpawnPlayer(propsCells[i]);
                continue;
            }
            if (RandomBetween(0, 5) == 0)
            {
                SpawnMinerals(propsCells[i]);  // Spawn Minerals
            }
        }
    }

    private void SpawnMinerals(MineItems mineral)
    {
        int mineralId = (int)mineral.itemId;
        PickaxeAble go = Instantiate(mineralPrefab, new Vector3(mineral.xPos, mineral.yPos), Quaternion.identity, this.transform);
        go.mineralId = mineralId;
        go.outputId = MineralsDatabase.GetMineralOutPutIdById(mineralId);
        go.HitPoints = MineralsDatabase.GetMineralHitPointsById(mineralId);
        if (RandomBetween(0, 10) == 0)
        {
            go.hasLadder = true;
        }
        else
        {
            go.hasLadder = false;
        }
    }

    private void SpawnPlayer(MineItems mineral)
    {
        player.transform.position = new Vector3(mineral.xPos, mineral.yPos);
    }

    private void SpawnExitMines(MineItems mineral)
    {
        GameObject go = Instantiate(Resources.Load("ExitMines"), new Vector3(mineral.xPos, mineral.yPos), Quaternion.identity, this.transform) as GameObject;
        go.name = "ExitMines";
    }

    private int RandomBetween(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
}

[System.Serializable]
public class MineItems
{
    public uint itemId;
    public int xPos;
    public int yPos;

    public MineItems()
    {

    }

    public MineItems(uint _id, int _xPos, int _yPos)
    {
        itemId = _id;
        xPos = _xPos;
        yPos = _yPos;
    }
}