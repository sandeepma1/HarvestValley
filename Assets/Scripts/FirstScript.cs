﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class FirstScript : MonoBehaviour
{
    [SerializeField]
    private int x = 6, y = 4, gap = 2;

    private Canvas[] canvas;
    private Camera mainCamera;
    private List<Fields> fields = new List<Fields>();
    private List<Buildings> buildings = new List<Buildings>();
    public List<FarmItems> playerInventory = new List<FarmItems>();

    private void Awake()
    {
        mainCamera = Camera.main;
        canvas = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].worldCamera = mainCamera;
        }
        IsNewGameStarted();
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
        playerInventory.Add(new FarmItems(0, 9)); //addding Wheat for the first level
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
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                fields.Add(new Fields(counter, 0, "Field", new Vector2(i * gap, -j * gap), 1, 0, -1, DateTime.UtcNow.ToString()));
                counter++;
            }
        }
        ES2.Save(fields, "AllFields");
    }

    private void CreateNewBuildings()
    {
        string[] nowTime = new string[GEM.maxQCount];
        int[] ids = new int[GEM.maxQCount];
        for (int i = 0; i < GEM.maxQCount; i++)
        {
            nowTime[i] = DateTime.UtcNow.ToString();
            ids[i] = -1;
        }

        buildings.Add(new Buildings(0, 2, "Building", 0, 2, ids, nowTime));

        ES2.Save(buildings, "AllBuildings");
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }
}