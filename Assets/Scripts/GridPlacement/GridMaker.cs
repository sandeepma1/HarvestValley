using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    [SerializeField]
    private int gridSizeX, gridSizeY;
    [SerializeField]
    private GameObject grid;

    void Start()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                print(i + " " + j);
                GameObject g = Instantiate(grid, this.transform);
                g.transform.localPosition = new Vector3(i, -j, 0);
            }
        }
    }
}