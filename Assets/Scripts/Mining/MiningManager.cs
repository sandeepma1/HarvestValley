using UnityEngine;
using System.Collections.Generic;
using CreativeSpore.SuperTilemapEditor;

public class MiningManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileMap;
    private List<MineItems> propsCells = new List<MineItems>();
    private float totalCells;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        int ran;
        for (int i = 0; i < propsCells.Count; i++)
        {
            if (propsCells[i].id == 0) // Spawn Exit Mines collider
            {
                SpawnExitMines(propsCells[i]);
                continue;
            }
            if (propsCells[i].id == 1) // Spawn Player
            {
                SpawnPlayer(propsCells[i]);
                continue;
            }
            ran = UnityEngine.Random.Range(0, 5);
            if (ran == 0)//  && propsCells[i].id != 0 && propsCells[i].id != 1)
            {
                SpawnMinerals(propsCells[i]);  // Spawn Minerals
            }
        }
    }

    private void SpawnMinerals(MineItems mineral)
    {
        GameObject go = Instantiate(Resources.Load("Rock1"), new Vector3(mineral.xPos, mineral.yPos), Quaternion.identity, this.transform) as GameObject;
        go.name = mineral.id.ToString();
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
}
[System.Serializable]
public class MineItems
{
    public uint id;
    public int xPos;
    public int yPos;

    public MineItems()
    {

    }

    public MineItems(uint _id, int _xPos, int _yPos)
    {
        id = _id;
        xPos = _xPos;
        yPos = _yPos;
    }
}