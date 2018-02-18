using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class BuildingManager : ManagerBase
{
    public static BuildingManager Instance = null;
    private List<Buildings> buildings = new List<Buildings>();
    public ClickableField[] FieldGO;

    // Use this for initialization
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //ClickableField.OnFieldClicked += OnClickableFieldClicked;
        OneTimeOnly();
        Init();
        //ToggleFieldSelector(false);
    }

    private void Init()
    {
        buildings = ES2.LoadList<Fields>("AllFields");
        FieldGO = new ClickableField[fields.Count];
        for (int i = 0; i < fields.Count; i++)
        {
            InitFields(fields[i]);
        }
        InvokeRepeating("SaveFields", 0, 5);
        InvokeRepeating("CheckForHarvest", 0, 1);
    }


    private void OneTimeOnly()
    {
        if (PlayerPrefs.GetInt("firstBuilding") == 0)
        {
            ES2.Delete("AllBuildings");
            print(ES2.Exists("AllBuildings"));

            buildings.Add(new Buildings(0, 0, "Building", new Vector2(0, 0), 1, 0, 0, -1, System.DateTime.UtcNow.ToString()));

            ES2.Save(buildings, "AllBuildings");
            PlayerPrefs.SetInt("buildingField", 1);
            StartCoroutine("RestartGame");
        }
    }
}

[System.Serializable]
public class Buildings  // iLIST
{
    public int id;
    public int buildingID;
    public string name;
    public int state;
    public int unlockedQueueSlots;
    public int itemID;
    public string dateTime;

    public Vector2 pos;
    public int level;

    public Buildings()
    {
    }

    public Buildings(int f_id, int f_fieldID, string f_name, Vector2 f_pos, int f_level, int f_state, int f_unlockedQueueSlots, int f_itemID, string f_dateTime)//, Queue <int>  f_itemID, Queue <string>  f_dateTime)
    {
        id = f_id;
        buildingID = f_fieldID;
        name = f_name;
        pos = f_pos;
        level = f_level;
        state = f_state;
        unlockedQueueSlots = f_unlockedQueueSlots;
        itemID = f_itemID;
        dateTime = f_dateTime;
    }
}

public enum BuildingState
{
    IDLE,
    WORKING,
    DONE
};

public enum BuildingStage
{
    SEED,
    SHRUB,
    PLANT,
    MATURE
};