﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameStartManager : MonoBehaviour
{
    [SerializeField]
    private int xField = 6, yField = 4, gapField = 2;
    private int xGrass = 12, yGrass = 12, gapGrass = 1;

    private void Start()
    {
        int gameStatus = PlayerPrefs.GetInt("gameStatus", 0);
        if (gameStatus == 0)
        {
            ResetGame();
            return;
        }
        else if (IsSavedFilesAvailable())
        {
            ResetGame();
            return;
        }
        else
        {
            LoadMainLevel();
        }
    }

    public void ResetGame()
    {
        StopAllCoroutines();
        //Note: This method will not work on WP8 or Metro.
        ES2.DeleteDefaultFolder();
        CreateNewGameSaves();
        print("Game Reset Started");
        StartCoroutine("RestartGame");
    }

    private bool IsSavedFilesAvailable()
    {
        if (!ES2.Exists("AllFields") || !ES2.Exists("AllGrass") || !ES2.Exists("AllLivestock") || !ES2.Exists("AllBuildings") ||
            !ES2.Exists("PlayerInventory") || !ES2.Exists("PlayerProfile"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CreateNewGameSaves()
    {
        CreateNewProfile();
        CreateNewInventory();
        CreateNewFields();
        CreateNewGrass();
        CreateNewLivestock();
        CreateNewBuildings();
        print("New game save created!!");
    }

    IEnumerator RestartGame()
    {
        PlayerPrefs.SetInt("gameStatus", 1);
        yield return new WaitForSeconds(1);
        LoadMainLevel();
    }

    private void LoadMainLevel()
    {
        print("Loading Main");
        SceneManager.LoadScene("Main");
    }

    #region Create New files for all types

    private void CreateNewInventory()
    {
        List<InventoryItems> playerInventory = new List<InventoryItems>();
        playerInventory.Add(new InventoryItems(0, 1)); //addding Wheat for the first level       
        ES2.Save(playerInventory, "PlayerInventory");
    }

    private void CreateNewProfile()
    {
        PlayersProfile playerProfile = new PlayersProfile("PlayerName", "MyFarm", 1, 0, 1000, 10, 50, DateTime.Now.ToString());
        ES2.Save(playerProfile, "PlayerProfile");
    }

    private void CreateNewFields()
    {
        List<Fields> fields = new List<Fields>();
        int counter = 0;
        for (int i = 0; i < xField; i++)
        {
            for (int j = 0; j < yField; j++)
            {
                fields.Add(new Fields(counter, 0, "Field", new Vector2(i * gapField, -j * gapField), 1, 0, -1, DateTime.Now.ToString()));
                counter++;
            }
        }
        ES2.Save(fields, "AllFields");
    }

    private void CreateNewGrass()
    {
        List<Grass> grass = new List<Grass>();
        int id = 0;
        for (int i = 0; i < xGrass; i++)
        {
            for (int j = 0; j < yGrass; j++)
            {
                grass.Add(new Grass(id, -1, new Vector2(i * gapGrass, -j * gapGrass)));
                id++;
            }
        }
        ES2.Save(grass, "AllGrass");
    }

    private void CreateNewLivestock()
    {
        List<LivestockClass> livestock = new List<LivestockClass>();
        livestock.Add(new LivestockClass(0, 4, 0, 0, 20, LivestockType.Chicken, DateTime.Now.ToString()));
        ES2.Save(livestock, "AllLivestock");
    }

    private void CreateNewBuildings()
    {
        List<Buildings> buildings = new List<Buildings>();

        string[] nowTime = new string[GEM.maxBuildingQueueCount];
        int[] ids = new int[GEM.maxBuildingQueueCount];
        for (int i = 0; i < GEM.maxBuildingQueueCount; i++)
        {
            nowTime[i] = DateTime.Now.ToString();
            ids[i] = -1;
        }

        buildings.Add(new Buildings(0, 2, "Bakery", 0, 2, new Vector2(0, 0), ids, nowTime));
        buildings.Add(new Buildings(1, 4, "Dairy", 0, 2, new Vector2(3, 0), ids, nowTime));

        ES2.Save(buildings, "AllBuildings");
    }

    #endregion

}