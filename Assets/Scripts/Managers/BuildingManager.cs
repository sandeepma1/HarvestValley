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
    [SerializeField]
    private ClickableBuilding buildingPrefab;
    public ClickableBuilding[] buildingsGO;

    // Use this for initialization
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ClickableBuilding.OnBuildingClicked += OnClickableFieldClicked;
        OneTimeOnly();
        Init();
        //ToggleFieldSelector(false);
    }

    private void OnClickableFieldClicked(int buildingID, int sourceID)
    {
        print("buildingID " + buildingID + " sourceID" + sourceID);
    }

    private void Init()
    {
        buildings = ES2.LoadList<Buildings>("AllBuildings");
        buildingsGO = new ClickableBuilding[buildings.Count];
        for (int i = 0; i < buildings.Count; i++)
        {
            InitFields(buildings[i]);
        }
        ////InvokeRepeating("SaveFields", 0, 5);
        //InvokeRepeating("CheckForHarvest", 0, 1);
    }

    private void InitFields(Buildings building)
    {
        buildingsGO[building.id] = Instantiate(buildingPrefab, transform);
        buildingsGO[building.id].gameObject.name = "Building" + building.id;
        buildingsGO[building.id].buildingSprite.sprite = AtlasBank.Instance.GetSprite(SourceDatabase.Instance.sources[building.buildingID].slug, AtlasType.Buildings);
        buildingsGO[building.id].buildingID = building.id;
        buildingsGO[building.id].sourceID = building.buildingID;
        buildingsGO[building.id].itemID = building.itemID;
        buildingsGO[building.id].state = (BuildingState)building.state;
        buildingsGO[building.id].dateTime = DateTime.Parse(building.dateTime);
        //CalculateFeildCrop(buildingsGO[building.id]);
    }

    private void OneTimeOnly()
    {
        if (PlayerPrefs.GetInt("firstBuilding") == 0)
        {
            ES2.Delete("AllBuildings");
            print(ES2.Exists("AllBuildings"));

            buildings.Add(new Buildings(0, 2, "Building", 0, 0, System.DateTime.UtcNow.ToString()));

            ES2.Save(buildings, "AllBuildings");
            PlayerPrefs.SetInt("firstBuilding", 1);
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
    public int itemID;
    public string dateTime;

    public Buildings()
    {
    }

    public Buildings(int f_id, int f_fieldID, string f_name, int f_state, int f_itemID, string f_dateTime)//, Queue <int>  f_itemID, Queue <string>  f_dateTime)
    {
        id = f_id;
        buildingID = f_fieldID;
        name = f_name;
        state = f_state;
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