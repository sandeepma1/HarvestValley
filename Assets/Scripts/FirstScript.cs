﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class FirstScript : MonoBehaviour
{
    public static FirstScript Instance = null;
    [SerializeField]
    private int xField = 6, yField = 4, gapField = 2;
    private int xGrass = 12, yGrass = 12, gapGrass = 1;


    private Canvas[] canvas;
    private Camera mainCamera;
    private List<Fields> fields = new List<Fields>();
    private List<Grass> grass = new List<Grass>();
    private List<Buildings> buildings = new List<Buildings>();
    public List<InventoryItems> playerInventory = new List<InventoryItems>();

    private void Awake()
    {
        Instance = this;
        IsNewGameStarted();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        canvas = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].worldCamera = mainCamera;
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyUp(KeyCode.R))
        {
            ResetGame();
        }
    }

    private void IsNewGameStarted()
    {
        if (!ES2.Exists("AllFields") ||
            !ES2.Exists("AllGrass") ||
            !ES2.Exists("AllBuildings") ||
            !ES2.Exists("PlayerInventory") ||
           !ES2.Exists("PlayerProfile"))
        {
            CreateNewGameSaves();
        }
    }

    private void CreateNewGameSaves()
    {
        CreateNewProfile();
        CreateNewFields();
        CreateNewGrass();
        CreateNewBuildings();
        CreateNewInventory();
        print("new game");
    }

    public void ResetGame()
    {
        //Note: This method will not work on WP8 or Metro.
        ES2.DeleteDefaultFolder();
        CreateNewGameSaves();
        print("rest game");
        StartCoroutine("RestartGame");
    }

    private void CreateNewInventory()
    {
        playerInventory.Add(new InventoryItems(0, 1)); //addding Wheat for the first level       
        ES2.Save(playerInventory, "PlayerInventory");
    }

    private void CreateNewProfile()
    {
        PlayerProfileManager.Instance.playerProfile = new PlayersProfile("PlayerName", "MyFarm", 1, 0, 1000, 10, 50, DateTime.UtcNow.ToString());
        ES2.Save(PlayerProfileManager.Instance.playerProfile, "PlayerProfile");
    }

    private void CreateNewFields()
    {
        int counter = 0;
        for (int i = 0; i < xField; i++)
        {
            for (int j = 0; j < yField; j++)
            {
                fields.Add(new Fields(counter, 0, "Field", new Vector2(i * gapField, -j * gapField), 1, 0, -1, DateTime.UtcNow.ToString()));
                counter++;
            }
        }
        ES2.Save(fields, "AllFields");
    }

    private void CreateNewGrass()
    {
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

    private void CreateNewBuildings()
    {
        string[] nowTime = new string[GEM.maxBuildingQueueCount];
        int[] ids = new int[GEM.maxBuildingQueueCount];
        for (int i = 0; i < GEM.maxBuildingQueueCount; i++)
        {
            nowTime[i] = DateTime.UtcNow.ToString();
            ids[i] = -1;
        }

        buildings.Add(new Buildings(0, 2, "Bakery", 0, 2, new Vector2(0, 0), ids, nowTime));
        buildings.Add(new Buildings(1, 3, "Dairy", 0, 2, new Vector2(3, 0), ids, nowTime));

        ES2.Save(buildings, "AllBuildings");
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }
}