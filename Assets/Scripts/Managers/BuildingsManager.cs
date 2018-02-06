using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.U2D;
using System.Collections;
using UnityEngine.SceneManagement;

public class BuildingsManager : Singleton<BuildingsManager>
{
    public int x = 5, y = 5, gap;
    [SerializeField]
    private Transform fieldSelector;
    public DraggableBuildings buildingPrefab;
    public GameObject MasterMenuGO = null, TimeRemainingMenu = null, FarmHarvestingMenu = null;
    public bool isFarmTimerEnabled = false;
    public List<Buildings> buildings = new List<Buildings>();
    public bool plantedOnSelectedfield = false;
    public int buildingSelectedID = -1;
    public DraggableBuildings[] BuildingsGO;
    System.TimeSpan remainingTime;
    int tempID = -1;
    public Sprite[] plantsSpriteBank;
    private Sprite[] buildingSpriteBank;
    public int itemSelectedID = -1;

    private void Awake()
    {
        //https://answers.unity.com/questions/1175266/getting-single-sprite-from-a-sprite-multiple.html
        plantsSpriteBank = Resources.LoadAll<Sprite>("Textures/Plants"); // loads all sprite from Resource folder
        buildingSpriteBank = Resources.LoadAll<Sprite>("Textures/Buildings");
    }

    private void Start()
    {
        DraggableBuildings.OnBuildingClicked += OnDraggableBuildingClicked;
        OneTimeOnly();
        Init();
        ToggleFieldSelector(false);
    }

    private void Update() // all long press logic	
    {
        //if (isTilePressed && BuildingsGO[mouseDownBuildingID].buildingID != 0)
        //{
        //    if (longPressTimer >= longPressTime)
        //    {
        //        isLongPress = true;
        //        longPressBuildingID = mouseDownBuildingID;
        //        mouseDownBuildingID = -1;
        //        isTilePressed = false;
        //        BuildingsGO[longPressBuildingID].isSelected = true;
        //        EnableOutlineOnSprite(longPressBuildingID);
        //        return;
        //    }
        //    longPressTimer += Time.deltaTime;
        //}
    }

    private void LateUpdate() //Mainly used to show time remaining
    {
        if (isFarmTimerEnabled)
        {
            ShowFarmLandTimeRemaining();
        }
        else
        {
            tempID = -1;
        }
    }

    private void Init()
    {
        buildings = ES2.LoadList<Buildings>("AllBuildings");
        BuildingsGO = new DraggableBuildings[buildings.Count];
        for (int i = 0; i < buildings.Count; i++)
        {
            InitBuildings(buildings[i]);
        }
        InvokeRepeating("SaveBuildings", 0, 5);
        InvokeRepeating("CheckForHarvest", 0, 1);
    }

    private void InitBuildings(Buildings building)
    {
        BuildingsGO[building.id] = Instantiate(buildingPrefab, transform);
        BuildingsGO[building.id].transform.localPosition = building.pos;
        BuildingsGO[building.id].gameObject.name = "Building" + building.id;
        BuildingsGO[building.id].buildingSprite.sprite = AtlasBank.Instance.GetSprite(SourceDatabase.Instance.sources[building.buildingID].slug, AtlasType.Buildings);
        BuildingsGO[building.id].buildingID = building.id;
        BuildingsGO[building.id].sourceID = building.buildingID;
        BuildingsGO[building.id].pos = building.pos;
        BuildingsGO[building.id].level = building.level;
        BuildingsGO[building.id].itemID = building.itemID;
        BuildingsGO[building.id].state = (BUILDINGS_STATE)building.state;
        BuildingsGO[building.id].unlockedQueueSlots = building.unlockedQueueSlots;
        BuildingsGO[building.id].dateTime = DateTime.Parse(building.dateTime);
        CalculateFeildCrop(BuildingsGO[building.id]);
    }

    private void CheckForHarvest()
    {
        for (int i = 0; i < BuildingsGO.Length; i++)
        {
            if (BuildingsGO[i] == null)
                return;

            switch (BuildingsGO[i].state)
            {
                case BUILDINGS_STATE.NONE:
                    break;
                case BUILDINGS_STATE.GROWING:
                    CalculateFeildCrop(BuildingsGO[i]);
                    break;
                case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                    break;
                default:
                    break;
            }
        }
    }

    private void CalculateFeildCrop(DraggableBuildings building)
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
        }
        else if (timeElapsedInSeconds >= divisionFactor * 2) //15 shrub
        {
            ChangeFarmPlantSprite(building, PLANT_STAGES.SHRUB);
        }
        else if (timeElapsedInSeconds >= divisionFactor) //7.5 plant
        {
            ChangeFarmPlantSprite(building, PLANT_STAGES.PLANT);
        }
        else if (timeElapsedInSeconds <= 0) // 0 mature
        {
            ChangeFarmPlantSprite(building, PLANT_STAGES.MATURE);
            building.state = BUILDINGS_STATE.WAITING_FOR_HARVEST;
            building.dateTime = new System.DateTime();
        }
    }

    private void ChangeFarmPlantSprite(DraggableBuildings building, PLANT_STAGES stages)
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

    private Sprite GetPlantSpriteFromBank(string spriteName)
    {
        Sprite sprite = new Sprite();
        sprite = plantsSpriteBank.Single(s => s.name == spriteName);
        if (sprite != null)
        {
            return plantsSpriteBank.Single(s => s.name == spriteName);
        }
        else
        {
            Debug.Log("Sprite Not Found " + spriteName);
            return new Sprite();
        }
    }

    //public void PlantItemsOnBuildings(int buildingID) // Planting Items
    //{
    //    if (BuildingsGO[buildingID].sourceID == 0)
    //    { // selected building is feild
    //        if (plantedOnSelectedfield || buildingSelectedID == buildingID)
    //        {
    //            if (PlayerInventoryManager.Instance.playerInventory[itemSelectedID].count >= 1)
    //            {
    //                BuildingsGO[buildingID].state = BUILDINGS_STATE.GROWING;
    //                BuildingsGO[buildingID].itemID = itemSelectedID;
    //                BuildingsGO[buildingID].dateTime = UTC.time.liveDateTime.AddMinutes(
    //                    ItemDatabase.Instance.items[itemSelectedID].timeRequiredInMins);
    //                // BuildingsGO[buildingID].spriteRenderer.color = Color.green;

    //                string plantName = ItemDatabase.Instance.items[itemSelectedID].name + "_0";

    //                BuildingsGO[buildingID].plantsSprite.sprite = GetPlantSpriteFromBank(plantName);
    //                PlayerInventoryManager.Instance.playerInventory[itemSelectedID].count--;
    //                SaveBuildings();
    //                plantedOnSelectedfield = true;
    //                buildingSelectedID = -1;
    //            }
    //        }
    //    }
    //    else
    //    { // if selected building is NOT feild
    //        if (buildingSelectedID == buildingID && DoesInventoryHasItems(buildingID))
    //        {
    //            DecrementItemsFromInventory();
    //            BuildingsGO[buildingID].state = BUILDINGS_STATE.GROWING;
    //            BuildingsGO[buildingID].itemID = itemSelectedID;
    //            BuildingsGO[buildingID].dateTime = UTC.time.liveDateTime.AddMinutes(
    //                ItemDatabase.Instance.items[itemSelectedID].timeRequiredInMins);
    //            BuildingsGO[buildingID].buildingSprite.color = Color.green;
    //        }
    //    }
    //}

    #region Planting Mode

    public void StartPlantingMode(int itemID)
    {
        for (int i = 0; i < BuildingsGO.Length; i++)
        {
            BuildingsGO[i].StartPlantingMode(itemID);
        }
        SaveBuildings();
        ToggleFieldSelector(false);
    }

    public void StopPlantingMode()
    {
        for (int i = 0; i < BuildingsGO.Length; i++)
        {
            BuildingsGO[i].StopPlantingMode();
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
            }
            else
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
            }
            else
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
            }
            else
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
            }
            else
            {
                needItems4 = -2;
            }
        }

        if (needItems1 >= -1 && needItems2 >= -1 && needItems3 >= -1 && needItems4 >= -1)
        {
            return true;
        }
        else
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
        PlayerInventoryManager.Instance.UpdateFarmItems(BuildingsGO[buildingID].itemID, 1);
        PlayerProfileManager.Instance.PlayerXPPointsAdd(ItemDatabase.Instance.items[BuildingsGO[buildingID].itemID].XPperYield);
        BuildingsGO[buildingID].state = BUILDINGS_STATE.NONE;
        BuildingsGO[buildingID].dateTime = new System.DateTime();
        BuildingsGO[buildingID].itemID = -1;
        BuildingsGO[buildingID].plantsSprite.sprite = new Sprite();
    }

    public void DisableAnyOpenMenus()
    {
        for (int i = 0; i < BuildingsGO.Length; i++)
        {
            if (BuildingsGO[i] != null)
            {
                BuildingsGO[i].isSelected = false;
            }
        }
    }

    public void ShowFarmLandTimeRemaining()
    {
        remainingTime = BuildingsGO[tempID].dateTime.Subtract(UTC.time.liveDateTime);
        TimeRemainingMenu.transform.position = BuildingsGO[tempID].transform.position;
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

    private void SelectBuilding(int buildingID)
    {
        if (BuildingsGO[buildingID].state == BUILDINGS_STATE.NONE)
        {
            ToggleFieldSelector(true);
            fieldSelector.SetParent(BuildingsGO[buildingID].transform);
            fieldSelector.transform.localPosition = Vector3.zero;
            buildingSelectedID = buildingID;
        }
        else
        {
            ToggleFieldSelector(false);
        }
    }

    private void ToggleFieldSelector(bool flag)
    {
        fieldSelector.gameObject.SetActive(flag);
    }

    #endregion

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Main");
    }

    #region OnMouse Functions

    private void OnDraggableBuildingClicked(int buildingID, int sourceID)
    {
        SelectBuilding(buildingID);
        switch (BuildingsGO[buildingID].state)
        {
            case BUILDINGS_STATE.NONE:
                UIMasterMenuManager.Instance.DisplayPlantSeedList(buildingID, BuildingsGO[buildingID].sourceID);
                break;
            case BUILDINGS_STATE.GROWING:
                tempID = buildingID;
                TimeRemainingMenu.SetActive(true);
                isFarmTimerEnabled = true;
                TimeRemainingMenu.transform.GetChild(0).GetComponent<TextMeshPro>().text =
                ItemDatabase.Instance.items[BuildingsGO[tempID].itemID].name.ToString();
                break;
            case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                CollectItemsOnBuildings(buildingID); // Directly collect/harvest on feild click
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
            item.pos = BuildingsGO[item.id].transform.localPosition;
            item.id = BuildingsGO[item.id].buildingID;
            item.buildingID = BuildingsGO[item.id].sourceID;
            item.level = BuildingsGO[item.id].level;
            item.state = (sbyte)BuildingsGO[item.id].state;
            item.unlockedQueueSlots = BuildingsGO[item.id].unlockedQueueSlots;
            item.itemID = BuildingsGO[item.id].itemID;
            item.dateTime = BuildingsGO[item.id].dateTime.ToString();
        }
        ES2.Save(buildings, "AllBuildings");
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