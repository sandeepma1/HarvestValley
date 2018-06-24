using UnityEngine;
using System.Collections.Generic;
using CreativeSpore.SuperTilemapEditor;
using HarvestValley.IO;

public class MiningManager : Singleton<MiningManager>
{
    private Tilemap propsTileMap;
    private TilemapGroup tilemapGroup;
    private List<MineItems> propsCells = new List<MineItems>();
    private float totalCells;
    private GameObject player;
    private PickaxeAble mineralPrefab;

    private void Start()
    {
        SpawnTilemMapGroup();
        propsTileMap = tilemapGroup.Tilemaps[tilemapGroup.Tilemaps.Count - 1];
        player = GameObject.FindGameObjectWithTag("Player");
        mineralPrefab = Resources.Load("mineral", typeof(PickaxeAble)) as PickaxeAble;
        propsTileMap.IsVisible = false;
        ReadPropsCells();
        GenerateProps();
    }

    private void SpawnTilemMapGroup()
    {
        print("In Mines Level " + GEM.currentMinesLevel);
        GameObject currentLevel = Resources.Load("MineLevels/MinesLevel" + GEM.currentMinesLevel) as GameObject;
        if (currentLevel == null)
        {
            // Todo: create new mining levels
            print("TODO: Create new levels");
            currentLevel = Instantiate(Resources.Load("MineLevels/MinesLevel2")) as GameObject;
        }
        else
        {
            currentLevel = Instantiate(currentLevel) as GameObject;
        }
        tilemapGroup = currentLevel.GetComponent<TilemapGroup>();
    }

    private void ReadPropsCells()
    {
        totalCells = propsTileMap.GridHeight * propsTileMap.GridWidth;
        for (int i = 0; i < propsTileMap.GridHeight; i++)
        {
            for (int j = 0; j < propsTileMap.GridWidth; j++)
            {
                if (propsTileMap.GetTileData(i, j) <= totalCells)
                {
                    propsCells.Add(new MineItems(propsTileMap.GetTileData(i, j), i, j));
                }
            }
        }
    }

    private void GenerateProps()
    {
        for (int i = 0; i < propsCells.Count; i++)
        {
            switch (propsCells[i].itemId)
            {
                case 0:// Load Farm Scene
                    SpwanItemFromResources("LoadFarm", propsCells[i].xPos, propsCells[i].yPos);
                    break;
                case 1:// // Player spawn
                    SpawnPlayer(propsCells[i]);
                    break;
                case 2:// Lift access level selection
                    SpwanItemFromResources("MineLevelSelector", propsCells[i].xPos, propsCells[i].yPos);
                    break;
                case 3://Load Mines Zero level
                    //LoadMinesZero
                    SpwanItemFromResources("LoadMinesZero", propsCells[i].xPos, propsCells[i].yPos);
                    break;
                case 4:// Mines level 1
                    SpwanItemFromResources("MiningFirstLevel", propsCells[i].xPos, propsCells[i].yPos);
                    break;
                case 5:// Not used
                    break;
                case 6:// Not used
                    break;
                case 7:// Not used
                    break;
                case 8:// Not used
                    break;
                case 9:// Not used
                    break;
                default: // Spawn Minerals
                    if (RandomBetween(0, 5) == 0)
                    {
                        SpawnMinerals(propsCells[i]);  // Spawn Minerals
                    }
                    break;
            }
        }
    }

    private void SpawnPlayer(MineItems mineral)
    {
        player.transform.position = new Vector3(mineral.xPos, mineral.yPos);
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

    private void SpwanItemFromResources(string name, float posX, float posY)
    {
        GameObject go = Instantiate(Resources.Load(name), new Vector3(posX, posY), Quaternion.identity, this.transform) as GameObject;
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