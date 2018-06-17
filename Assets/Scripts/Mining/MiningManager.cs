using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class MiningManager : MonoBehaviour
{
    public Tilemap tileMap;
    // Use this for initialization
    void Start()
    {
        float totalCells = tileMap.GridHeight * tileMap.GridWidth;
        for (int i = 0; i < tileMap.GridHeight; i++)
        {
            for (int j = 0; j < tileMap.GridWidth; j++)
            {
                if (tileMap.GetTileData(i, j) <= totalCells)
                {
                    print(tileMap.GetTileData(i, j));
                }
                //print(tileMap.GetTileData(i, j));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
