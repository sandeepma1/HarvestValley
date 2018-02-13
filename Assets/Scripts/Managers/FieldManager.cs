using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class FieldManager : ManagerBase
{
    public static FieldManager Instance = null;
    [SerializeField]
    private Transform fieldSelector;
    [SerializeField]
    private ClickableField buildingPrefab;
    [SerializeField]
    private GameObject TimeRemainingMenu = null;

    public bool plantedOnSelectedfield = false;
    public int buildingSelectedID = -1;
    public ClickableField[] FieldGO;
    public int itemSelectedID = -1; // TODO: delete this asap
    public int currentSelectedFieldID = -1;
    public int currentlySelectedSourceID = -1;

    private TimeSpan remainingTime;
    private int tempID = -1;
    private List<Buildings> buildings = new List<Buildings>();
    private bool isFarmTimerEnabled = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ClickableField.OnBuildingClicked += OnClickableFieldClicked;
        OneTimeOnly();
        Init();
        ToggleFieldSelector(false);
    }

    private void LateUpdate() //Mainly used to show time remaining
    {
        if (isFarmTimerEnabled)
        {
            ShowFarmLandTimeRemaining();
        } else
        {
            tempID = -1;
        }
    }

    private void Init()
    {
        buildings = ES2.LoadList<Buildings>("AllBuildings");
        FieldGO = new ClickableField[buildings.Count];
        for (int i = 0; i < buildings.Count; i++)
        {
            InitBuildings(buildings[i]);
        }
        InvokeRepeating("SaveBuildings", 0, 5);
        InvokeRepeating("CheckForHarvest", 0, 1);
    }

    private void InitBuildings(Buildings building)
    {
        FieldGO[building.id] = Instantiate(buildingPrefab, transform);
        FieldGO[building.id].transform.localPosition = building.pos;
        FieldGO[building.id].gameObject.name = "Building" + building.id;
        FieldGO[building.id].buildingSprite.sprite = AtlasBank.Instance.GetSprite(SourceDatabase.Instance.sources[building.buildingID].slug, AtlasType.Buildings);
        FieldGO[building.id].buildingID = building.id;
        FieldGO[building.id].sourceID = building.buildingID;
        FieldGO[building.id].pos = building.pos;
        FieldGO[building.id].level = building.level;
        FieldGO[building.id].itemID = building.itemID;
        FieldGO[building.id].state = (BUILDINGS_STATE)building.state;
        FieldGO[building.id].unlockedQueueSlots = building.unlockedQueueSlots;
        FieldGO[building.id].dateTime = DateTime.Parse(building.dateTime);
        CalculateFeildCrop(FieldGO[building.id]);
    }

    private void CheckForHarvest()
    {
        for (int i = 0; i < FieldGO.Length; i++)
        {
            if (FieldGO[i] == null)
                return;

            switch (FieldGO[i].state)
            {
                case BUILDINGS_STATE.NONE:
                    break;
                case BUILDINGS_STATE.GROWING:
                    CalculateFeildCrop(FieldGO[i]);
                    break;
                case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                    break;
                default:
                    break;
            }
        }
    }

    private void CalculateFeildCrop(ClickableField building)
    {
        if (building.itemID < 0)
        {
            return;
        }

        TimeSpan timeElapsed = building.dateTime - UTC.time.liveDateTime;
        float timeElapsedInSeconds = (float)timeElapsed.TotalSeconds;
        float divisionFactor = (ItemDatabase.Instance.items[building.sourceID].timeRequiredInMins * 60) / 4;

        if (timeElapsedInSeconds >= divisionFactor * 3) //22.5 seed
        {
            ChangeFarmPlantSprite(building, PLANT_STAGES.SEED);
        } else if (timeElapsedInSeconds >= divisionFactor * 2) //15 shrub
        {
            ChangeFarmPlantSprite(building, PLANT_STAGES.SHRUB);
        } else if (timeElapsedInSeconds >= divisionFactor) //7.5 plant
        {
            ChangeFarmPlantSprite(building, PLANT_STAGES.PLANT);
        } else if (timeElapsedInSeconds <= 0) // 0 mature
        {
            ChangeFarmPlantSprite(building, PLANT_STAGES.MATURE);
            building.state = BUILDINGS_STATE.WAITING_FOR_HARVEST;
            building.dateTime = new System.DateTime();
        }
    }

    private void ChangeFarmPlantSprite(ClickableField building, PLANT_STAGES stages)
    {
        string itemSlug = ItemDatabase.Instance.items[building.itemID].slug;
        switch (stages)
        {
            case PLANT_STAGES.SEED:
                if (building.plantsSprite.sprite != AtlasBank.Instance.GetSprite(itemSlug + "_0", AtlasType.Farming))
                {
                    building.plantsSprite.sprite = AtlasBank.Instance.GetSprite(itemSlug + "_0", AtlasType.Farming);
                }
                break;
            case PLANT_STAGES.SHRUB:
                if (building.plantsSprite.sprite != AtlasBank.Instance.GetSprite(itemSlug + "_1", AtlasType.Farming))
                {
                    building.plantsSprite.sprite = AtlasBank.Instance.GetSprite(itemSlug + "_1", AtlasType.Farming);
                }
                break;
            case PLANT_STAGES.PLANT:
                if (building.plantsSprite.sprite != AtlasBank.Instance.GetSprite(itemSlug + "_2", AtlasType.Farming))
                {
                    building.plantsSprite.sprite = AtlasBank.Instance.GetSprite(itemSlug + "_2", AtlasType.Farming);
                }
                break;
            case PLANT_STAGES.MATURE:
                if (building.plantsSprite.sprite != AtlasBank.Instance.GetSprite(itemSlug + "_3", AtlasType.Farming))
                {
                    building.plantsSprite.sprite = AtlasBank.Instance.GetSprite(itemSlug + "_3", AtlasType.Farming);
                }
                break;
            default:
                break;
        }
    }

    #region Planting Mode

    public void StartPlantingMode(int itemID)
    {
        for (int i = 0; i < FieldGO.Length; i++)
        {
            if (FieldGO[i].state == BUILDINGS_STATE.NONE)
            {
                FieldGO[i].StartPlantingMode(itemID);
            }
        }
        SaveBuildings();
        ToggleFieldSelector(false);
    }

    public void StopPlantingMode()
    {
        for (int i = 0; i < FieldGO.Length; i++)
        {
            FieldGO[i].StopPlantingMode();
        }
        //ToggleFieldSelector(true);
    }

    #endregion

    public bool DoesInventoryHasItems(int itemID)
    {
        int needItems1 = -1;
        int needItems2 = -1;
        int needItems3 = -1;
        int needItems4 = -1;

        if (ItemDatabase.Instance.items[itemSelectedID].needID1 >= 0)
        {
            if (PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID1].count >=
                ItemDatabase.Instance.items[itemSelectedID].needAmount1)
            {
                needItems1 = 0;
                print("1 ok");
            } else
            {
                needItems1 = -2;
            }
        }
        if (ItemDatabase.Instance.items[itemSelectedID].needID2 >= 0)
        {
            if (PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID2].count >=
                ItemDatabase.Instance.items[itemSelectedID].needAmount2)
            {
                needItems2 = 0;
                print("1 ok");
            } else
            {
                needItems2 = -2;
            }
        }
        if (ItemDatabase.Instance.items[itemSelectedID].needID3 >= 0)
        {
            if (PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID3].count >=
                ItemDatabase.Instance.items[itemSelectedID].needAmount3)
            {
                needItems3 = 0;
                print("1 ok");
            } else
            {
                needItems3 = -2;
            }
        }
        if (ItemDatabase.Instance.items[itemSelectedID].needID4 >= 0)
        {
            if (PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID4].count >=
                ItemDatabase.Instance.items[itemSelectedID].needAmount4)
            {
                needItems4 = 0;
                print("1 ok");
            } else
            {
                needItems4 = -2;
            }
        }

        if (needItems1 >= -1 && needItems2 >= -1 && needItems3 >= -1 && needItems4 >= -1)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void DecrementItemsFromInventory()
    {
        if (ItemDatabase.Instance.items[itemSelectedID].needID1 >= 0)
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID1].count =
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID1].count - ItemDatabase.Instance.items[itemSelectedID].needAmount1;

        if (ItemDatabase.Instance.items[itemSelectedID].needID2 >= 0)
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID2].count =
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID2].count - ItemDatabase.Instance.items[itemSelectedID].needAmount2;

        if (ItemDatabase.Instance.items[itemSelectedID].needID3 >= 0)
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID3].count =
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID3].count - ItemDatabase.Instance.items[itemSelectedID].needAmount3;

        if (ItemDatabase.Instance.items[itemSelectedID].needID4 >= 0)
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID4].count =
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[itemSelectedID].needID4].count - ItemDatabase.Instance.items[itemSelectedID].needAmount4;

    }

    public void CollectItemsOnBuildings(int buildingID) //Collecting Items on buildings
    {
        PlayerInventoryManager.Instance.UpdateFarmItems(FieldGO[buildingID].itemID, 1);
        PlayerProfileManager.Instance.PlayerXPPointsAdd(ItemDatabase.Instance.items[FieldGO[buildingID].itemID].XPperYield);
        FieldGO[buildingID].state = BUILDINGS_STATE.NONE;
        FieldGO[buildingID].dateTime = new System.DateTime();
        FieldGO[buildingID].itemID = -1;
        FieldGO[buildingID].plantsSprite.sprite = new Sprite();
    }

    public void DisableAnyOpenMenus()
    {
        for (int i = 0; i < FieldGO.Length; i++)
        {
            if (FieldGO[i] != null)
            {
                FieldGO[i].isSelected = false;
            }
        }
    }

    public void ShowFarmLandTimeRemaining()
    {
        remainingTime = FieldGO[tempID].dateTime.Subtract(UTC.time.liveDateTime);
        TimeRemainingMenu.transform.position = FieldGO[tempID].transform.position;
        if (remainingTime <= new System.TimeSpan(360, 0, 0, 0))
        { //> 1year
            TimeRemainingMenu.transform.GetChild(1).GetComponent<TextMeshPro>().text = remainingTime.Days.ToString() + "d " + remainingTime.Hours.ToString() + "h";
        }
        if (remainingTime <= new System.TimeSpan(1, 0, 0, 0))
        { //> 1day
            TimeRemainingMenu.transform.GetChild(1).GetComponent<TextMeshPro>().text = remainingTime.Hours.ToString() + "h " + remainingTime.Minutes.ToString() + "m";
        }
        if (remainingTime <= new System.TimeSpan(0, 1, 0, 0))
        { //> 1hr
            TimeRemainingMenu.transform.GetChild(1).GetComponent<TextMeshPro>().text = remainingTime.Minutes.ToString() + "m " + remainingTime.Seconds.ToString() + "s";
        }
        if (remainingTime <= new System.TimeSpan(0, 0, 1, 0))
        { // 1min
            TimeRemainingMenu.transform.GetChild(1).GetComponent<TextMeshPro>().text = remainingTime.Seconds.ToString() + "s";
        }
        if (remainingTime <= new System.TimeSpan(0, 0, 0, 0))
        { // 1min
            TimeRemainingMenu.SetActive(false);
        }
    }

    public void AddNewBuilding(Vector2 pos, int buildingID)
    {
        buildings.Add(new Buildings(buildings.Count + 1, buildingID, SourceDatabase.Instance.sources[buildingID].sourceID.ToString(), pos,
            1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
        ES2.Save(buildings, "AllBuildings");
        InitBuildings(buildings[buildings.Count - 1]);
    }

    private void OneTimeOnly()
    {
        if (PlayerPrefs.GetInt("firstBuilding") == 0)
        {
            ES2.Delete("AllBuildings");
            print(ES2.Exists("AllBuildings"));
            int counter = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    buildings.Add(new Buildings(counter, 0, "Field", new Vector2(i * gap, -j * gap), 1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
                    counter++;
                }
            }
            ES2.Save(buildings, "AllBuildings");
            PlayerPrefs.SetInt("firstBuilding", 1);
            StartCoroutine("RestartGame");
        }
    }

    #region Field Selector

    private void SelectField()
    {
        if (FieldGO[currentSelectedFieldID].state == BUILDINGS_STATE.NONE)
        {
            ToggleFieldSelector(true);
            fieldSelector.SetParent(FieldGO[currentSelectedFieldID].transform);
            fieldSelector.transform.localPosition = Vector3.zero;
            buildingSelectedID = currentSelectedFieldID;
        } else
        {
            ToggleFieldSelector(false);
        }
    }

    private void ToggleFieldSelector(bool flag)
    {
        fieldSelector.gameObject.SetActive(flag);
    }

    internal void DeselectField()
    {
        if (currentSelectedFieldID == -1)
        {
            return;
        }
        ToggleFieldSelector(false);
        currentSelectedFieldID = -1;
    }

    #endregion

    #region OnMouse Functions

    private void OnClickableFieldClicked(int fieldID, int sourceID)
    {
        currentSelectedFieldID = fieldID;
        currentlySelectedSourceID = sourceID;

        SelectField();
        switch (FieldGO[fieldID].state)
        {
            case BUILDINGS_STATE.NONE:
                MenuManager.Instance.DisplayMenu(MenuNames.SeedList, MenuOpeningType.CloseAll);
                break;
            case BUILDINGS_STATE.GROWING:
                tempID = fieldID;
                TimeRemainingMenu.SetActive(true);
                isFarmTimerEnabled = true;
                TimeRemainingMenu.transform.GetChild(0).GetComponent<TextMeshPro>().text =
                ItemDatabase.Instance.items[FieldGO[tempID].itemID].name.ToString();
                break;
            case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                CollectItemsOnBuildings(fieldID); // Directly collect/harvest on feild click
                break;
            default:
                break;
        }
    }

    #endregion

    void SaveBuildings()
    {
        foreach (var item in buildings)
        {
            item.pos = FieldGO[item.id].transform.localPosition;
            item.id = FieldGO[item.id].buildingID;
            item.buildingID = FieldGO[item.id].sourceID;
            item.level = FieldGO[item.id].level;
            item.state = (sbyte)FieldGO[item.id].state;
            item.unlockedQueueSlots = FieldGO[item.id].unlockedQueueSlots;
            item.itemID = FieldGO[item.id].itemID;
            item.dateTime = FieldGO[item.id].dateTime.ToString();
        }
        ES2.Save(buildings, "AllBuildings");
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Main");
    }
}

[System.Serializable]
public class Buildings  // iLIST
{
    public int id;
    public int buildingID;
    public string name;
    public Vector2 pos;
    public int level;
    public int state;
    public int unlockedQueueSlots;
    public int itemID;
    public string dateTime;

    public Buildings()
    {
    }

    public Buildings(int f_id, int f_buildingID, string f_name, Vector2 f_pos, int f_level, int f_state, int f_unlockedQueueSlots, int f_itemID, string f_dateTime)//, Queue <int>  f_itemID, Queue <string>  f_dateTime)
    {
        id = f_id;
        buildingID = f_buildingID;
        name = f_name;
        pos = f_pos;
        level = f_level;
        state = f_state;
        unlockedQueueSlots = f_unlockedQueueSlots;
        itemID = f_itemID;
        dateTime = f_dateTime;
    }
}

public enum BUILDINGS_STATE
{
    NONE,
    GROWING,
    WAITING_FOR_HARVEST
};

public enum PLANT_STAGES
{
    SEED,
    SHRUB,
    PLANT,
    MATURE
};