using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager m_instance = null;
    public GameObject buildingPrefab;
    public GameObject MasterMenuGO = null, TimeRemainingMenu = null, FarmHarvestingMenu = null;
    public bool isFarmTimerEnabled = false;
    public List<Buildings> buildings = new List<Buildings>();
    public List<AAA> aaa = new List<AAA>();
    public bool plantedOnSelectedfield = false;
    public int buildingSelectedID = -1;
    public GameObject[] BuildingsGO;
    System.TimeSpan remainingTime;
    int tempID = -1, longPressBuildingID = -1, mouseDownBuildingID = -1;
    bool isLongPress = false;
    bool isTilePressed = false;
    float longPressTime = 0.5f, longPressTimer = 0f;

    void Awake()
    {
        m_instance = this;
        OneTimeOnly();
        Init();
    }

    void Init()
    {
        buildings = ES2.LoadList<Buildings>("AllBuildings");
        //BuildingsGO = new GameObject[buildings.Count];
        BuildingsGO = new GameObject[99];
        foreach (var building in buildings)
        {
            InitBuildings(building);
        }
        InvokeRepeating("SaveBuildings", 0, 5);
    }



    public void DisplayMasterMenu(int b_ID) // Display field Crop Menu
    {
        MasterMenuManager.m_instance.PopulateItemsInMasterMenu(BuildingsGO[b_ID].GetComponent<DraggableBuildings>().buildingID);
        IGMMenu.m_instance.DisableAllMenus();
        buildingSelectedID = b_ID;

        //Animatin Stuff
        MasterMenuGO.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        MasterMenuGO.transform.position = BuildingsGO[b_ID].transform.position;
        MasterMenuGO.SetActive(true);
        LeanTween.scale(MasterMenuGO.transform.GetChild(0).gameObject, new Vector3(2, 2, 2), 0.2f, IGMMenu.m_instance.ease);
    }

    public void PlantItemsOnBuildings(int buildingID) // Planting Items
    {
        if (MasterMenuManager.m_instance.isItemSelected == true)
        {
            if (BuildingsGO[buildingID].GetComponent<DraggableBuildings>().buildingID == 0)
            { // selected building is feild
                if (plantedOnSelectedfield || buildingSelectedID == buildingID)
                {
                    if (PlayerInventoryManager.m_instance.playerInventory[MasterMenuManager.m_instance.itemSelectedID].count >= 1)
                    {
                        BuildingsGO[buildingID].GetComponent<DraggableBuildings>().state = BUILDINGS_STATE.GROWING;
                        BuildingsGO[buildingID].GetComponent<DraggableBuildings>().itemID = MasterMenuManager.m_instance.itemSelectedID;
                        BuildingsGO[buildingID].GetComponent<DraggableBuildings>().dateTime = UTC.time.liveDateTime.AddMinutes(
                            ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].timeRequiredInMins);
                        BuildingsGO[buildingID].GetComponent<SpriteRenderer>().color = Color.green;
                        PlayerInventoryManager.m_instance.playerInventory[MasterMenuManager.m_instance.itemSelectedID].count--;
                        MasterMenuManager.m_instance.UpdateSeedValue();
                        SaveBuildings();
                        plantedOnSelectedfield = true;
                        buildingSelectedID = -1;
                    }
                }
            } else
            { // if selected building is NOT feild
                if (buildingSelectedID == buildingID && DoesInventoryHasItems(buildingID))
                {
                    DecrementItemsFromInventory();
                    BuildingsGO[buildingID].GetComponent<DraggableBuildings>().state = BUILDINGS_STATE.GROWING;
                    BuildingsGO[buildingID].GetComponent<DraggableBuildings>().itemID = MasterMenuManager.m_instance.itemSelectedID;
                    BuildingsGO[buildingID].GetComponent<DraggableBuildings>().dateTime = UTC.time.liveDateTime.AddMinutes(
                        ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].timeRequiredInMins);
                    BuildingsGO[buildingID].GetComponent<SpriteRenderer>().color = Color.green;

                }
            }
        }
    }

    public bool DoesInventoryHasItems(int itemID)
    {
        int needItems1 = -1;
        int needItems2 = -1;
        int needItems3 = -1;
        int needItems4 = -1;

        if (ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID1 >= 0)
        {
            if (PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID1].count >=
                ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needAmount1)
            {
                needItems1 = 0;
                print("1 ok");
            } else
            {
                needItems1 = -2;
            }
        }
        if (ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID2 >= 0)
        {
            if (PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID2].count >=
                ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needAmount2)
            {
                needItems2 = 0;
                print("1 ok");
            } else
            {
                needItems2 = -2;
            }
        }
        if (ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID3 >= 0)
        {
            if (PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID3].count >=
                ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needAmount3)
            {
                needItems3 = 0;
                print("1 ok");
            } else
            {
                needItems3 = -2;
            }
        }
        if (ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID4 >= 0)
        {
            if (PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID4].count >=
                ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needAmount4)
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
        if (ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID1 >= 0)
            PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID1].count =
            PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID1].count - ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needAmount1;

        if (ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID2 >= 0)
            PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID2].count =
            PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID2].count - ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needAmount2;

        if (ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID3 >= 0)
            PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID3].count =
            PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID3].count - ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needAmount3;

        if (ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID4 >= 0)
            PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID4].count =
            PlayerInventoryManager.m_instance.playerInventory[ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needID4].count - ItemDatabase.m_instance.items[MasterMenuManager.m_instance.itemSelectedID].needAmount4;

    }

    public void HarvestCropOnFarmLand(int buildingID) // Harvesting Seeds calls only on farms
    {
        if (HarvestMenuManager.m_instance.isScytheSelected == true)
        {
            // TODO Heavy update required for field Level Based cals*******************
            // only 2 items are added in storage
            //			print (FarmLands [buildingID].GetComponent <FarmLands> ().itemID);		 
            PlayerInventoryManager.m_instance.UpdateFarmItems(Convert.ToInt32(BuildingsGO[buildingID].GetComponent<DraggableBuildings>().itemID), 2);
            PlayerProfileManager.m_instance.PlayerXPPointsAdd(ItemDatabase.m_instance.items[BuildingsGO[buildingID].GetComponent<DraggableBuildings>().itemID].XP);
            BuildingsGO[buildingID].GetComponent<DraggableBuildings>().state = BUILDINGS_STATE.NONE;

            BuildingsGO[buildingID].GetComponent<DraggableBuildings>().dateTime = new System.DateTime();
            BuildingsGO[buildingID].GetComponent<DraggableBuildings>().itemID = -1;

            BuildingsGO[buildingID].GetComponent<SpriteRenderer>().color = Color.white;
            HarvestMenuManager.m_instance.ToggleDisplayHarvestingMenu();
        }
    }

    public void ShowReadyToHarvestCropsMenu(int buildingID) // Display Harvesting Menu
    {
        IGMMenu.m_instance.DisableAllMenus();
        FarmHarvestingMenu.transform.position = BuildingsGO[buildingID].transform.position;
        FarmHarvestingMenu.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        FarmHarvestingMenu.SetActive(true);
        LeanTween.scale(FarmHarvestingMenu, Vector3.one, 0.2f, IGMMenu.m_instance.ease);
    }

    public void CollectItemsOnBuildings(int buildingID) //Collecting Items on buildings
    {
        PlayerInventoryManager.m_instance.UpdateFarmItems(BuildingsGO[buildingID].GetComponent<DraggableBuildings>().itemID, 1);
        PlayerProfileManager.m_instance.PlayerXPPointsAdd(ItemDatabase.m_instance.items[BuildingsGO[buildingID].GetComponent<DraggableBuildings>().itemID].XP);
        BuildingsGO[buildingID].GetComponent<DraggableBuildings>().state = BUILDINGS_STATE.NONE;
        BuildingsGO[buildingID].GetComponent<DraggableBuildings>().dateTime = new System.DateTime();
        BuildingsGO[buildingID].GetComponent<DraggableBuildings>().itemID = -1;
        BuildingsGO[buildingID].GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void DisableAnyOpenMenus()
    {
        for (int i = 0; i < BuildingsGO.Length; i++)
        {
            if (BuildingsGO[i] != null)
            {
                BuildingsGO[i].GetComponent<DraggableBuildings>().isSelected = false;
                DisableOutlineOnSprite(i);
            }
        }
        isLongPress = false;
    }

    public void ShowFarmLandTimeRemaining()
    {
        remainingTime = BuildingsGO[tempID].GetComponent<DraggableBuildings>().dateTime.Subtract(UTC.time.liveDateTime);
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

    void LateUpdate() //Mainly used to show time remaining
    {
        if (isFarmTimerEnabled)
        {
            ShowFarmLandTimeRemaining();
        } else
        {
            tempID = -1;
        }
        foreach (var item in BuildingsGO)
        {  //Main loop for checking all buildings time
            if (item != null && item.GetComponent<DraggableBuildings>().state == BUILDINGS_STATE.GROWING && item.GetComponent<DraggableBuildings>().dateTime.Subtract(UTC.time.liveDateTime) <= new System.TimeSpan(0, 0, 0))
            {
                item.GetComponent<DraggableBuildings>().state = BUILDINGS_STATE.WAITING_FOR_HARVEST;
                item.GetComponent<DraggableBuildings>().dateTime = new System.DateTime();
                item.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    void Update() // all long press logic	
    {
        if (isTilePressed)
        {    // all long press logic		
            if (longPressTimer >= longPressTime)
            {
                isLongPress = true;
                longPressBuildingID = mouseDownBuildingID;
                mouseDownBuildingID = -1;
                isTilePressed = false;
                BuildingsGO[longPressBuildingID].GetComponent<DraggableBuildings>().isSelected = true;
                EnableOutlineOnSprite(longPressBuildingID);
                return;
            }
            longPressTimer += Time.deltaTime;
        }
    }

    void EnableOutlineOnSprite(int selectedFieldID)
    {
        BuildingsGO[selectedFieldID].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    }

    void DisableOutlineOnSprite(int selectedFieldID)
    {
        BuildingsGO[selectedFieldID].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
    }

    public void AddNewBuilding(Vector2 pos, int buildingID)
    {
        buildings.Add(new Buildings(buildings.Count + 1, buildingID, BuildingDatabase.m_instance.buildingInfo[buildingID].name.ToString(), pos,
            1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
        ES2.Save(buildings, "AllBuildings");
        InitBuildings(buildings[buildings.Count - 1]);
    }

    #region OnMouse Functions

    void OneTimeOnly()
    {
        if (PlayerPrefs.GetInt("firstBuilding") <= 0)
        {
            ES2.Delete("AllBuildings");
            buildings.Add(new Buildings(0, 0, "Field", new Vector2(0, 0), 1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
            buildings.Add(new Buildings(1, 0, "Field", new Vector2(1, 0), 1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
            buildings.Add(new Buildings(2, 0, "Field", new Vector2(2, 0), 1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
            buildings.Add(new Buildings(3, 0, "Field", new Vector2(3, 0), 1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
            buildings.Add(new Buildings(4, 0, "Field", new Vector2(4, 0), 1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
            buildings.Add(new Buildings(5, 1, "Bakery", new Vector2(1, 1), 1, 0, 2, -1, System.DateTime.UtcNow.ToString()));
            buildings.Add(new Buildings(6, 2, "FeedMill", new Vector2(2, 1), 1, 0, 2, -1, System.DateTime.UtcNow.ToString()));
            buildings.Add(new Buildings(7, 3, "Dairy", new Vector2(3, 1), 1, 0, 2, -1, System.DateTime.UtcNow.ToString()));
            ES2.Save(buildings, "AllBuildings");
            PlayerPrefs.SetInt("firstBuilding", 1);
        }
    }

    public void InitBuildings(Buildings building)
    {
        BuildingsGO[building.id] = Instantiate(buildingPrefab, this.transform);
        BuildingsGO[building.id].transform.localPosition = building.pos;
        BuildingsGO[building.id].gameObject.name = "Building" + building.id;
        BuildingsGO[building.id].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Buildings/" + building.name);
        BuildingsGO[building.id].GetComponent<DraggableBuildings>().id = building.id;
        BuildingsGO[building.id].GetComponent<DraggableBuildings>().buildingID = building.buildingID;
        BuildingsGO[building.id].GetComponent<DraggableBuildings>().pos = building.pos;
        BuildingsGO[building.id].GetComponent<DraggableBuildings>().level = building.level;
        BuildingsGO[building.id].GetComponent<DraggableBuildings>().itemID = building.itemID;
        BuildingsGO[building.id].GetComponent<DraggableBuildings>().state = (BUILDINGS_STATE)building.state;
        BuildingsGO[building.id].GetComponent<DraggableBuildings>().unlockedQueueSlots = building.unlockedQueueSlots;
        DisableOutlineOnSprite(building.id);
        switch (BuildingsGO[building.id].GetComponent<DraggableBuildings>().state)
        {
            case BUILDINGS_STATE.NONE:
                BuildingsGO[building.id].GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case BUILDINGS_STATE.GROWING:
                BuildingsGO[building.id].GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                BuildingsGO[building.id].GetComponent<SpriteRenderer>().color = Color.red;
                break;
            default:
                break;
        }
        BuildingsGO[building.id].GetComponent<DraggableBuildings>().dateTime = DateTime.Parse(building.dateTime);
    }

    public void CallParentOnMouseDown(int buildingID)
    {
        isTilePressed = true;
        longPressTimer = 0;
        mouseDownBuildingID = buildingID;
        if (buildingID != longPressBuildingID && longPressBuildingID != -1)
        {
            BuildingsGO[longPressBuildingID].GetComponent<DraggableBuildings>().isSelected = false;
            DisableOutlineOnSprite(longPressBuildingID);
            isLongPress = false;
        }
    }

    public void CallParentOnMouseUp(int buildingID)
    {
        isTilePressed = false;
        mouseDownBuildingID = -1;
        if (!isLongPress)
        {
            switch (BuildingsGO[buildingID].GetComponent<DraggableBuildings>().state)
            {
                case BUILDINGS_STATE.NONE:
                    DisplayMasterMenu(buildingID);
                    break;
                case BUILDINGS_STATE.GROWING:
                    tempID = buildingID;
                    IGMMenu.m_instance.DisableAllMenus();
                    TimeRemainingMenu.SetActive(true);
                    isFarmTimerEnabled = true;
                    TimeRemainingMenu.transform.GetChild(0).GetComponent<TextMeshPro>().text =
                    ItemDatabase.m_instance.items[BuildingsGO[tempID].GetComponent<DraggableBuildings>().itemID].name.ToString();

                    break;
                case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                    if (BuildingsGO[buildingID].GetComponent<DraggableBuildings>().buildingID == 0)
                    { // if field selected
                        ShowReadyToHarvestCropsMenu(buildingID);
                    } else
                    {
                        CollectItemsOnBuildings(buildingID);
                    }
                    break;
                default:
                    break;
            }
        } else
        {
            if (buildingID != longPressBuildingID)
            {
                BuildingsGO[longPressBuildingID].GetComponent<DraggableBuildings>().isSelected = false;
                DisableOutlineOnSprite(longPressBuildingID);
                isLongPress = false;
            }
        }
    }

    public void CallParentOnMouseEnter(int buildingID)
    {
        switch (BuildingsGO[buildingID].GetComponent<DraggableBuildings>().state)
        {
            case BUILDINGS_STATE.NONE:
                PlantItemsOnBuildings(buildingID);
                break;
            case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                if (BuildingsGO[buildingID].GetComponent<DraggableBuildings>().buildingID == 0)
                { // if field selected
                    HarvestCropOnFarmLand(buildingID);
                }
                break;
            default:
                break;
        }
    }

    public void CallParentOnMouseDrag(int buildingID)
    {
        if (BuildingsGO[buildingID].GetComponent<DraggableBuildings>().isSelected)
        {
            BuildingsGO[buildingID].transform.position = new Vector3(Mathf.Round(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0)).x),
                Mathf.Round(Camera.main.ScreenToWorldPoint(new Vector3(0, Input.mousePosition.y, 0)).y), 0);
        }
    }

    #endregion

    void SaveBuildings()
    {
        foreach (var item in buildings)
        {
            item.pos = BuildingsGO[item.id].transform.localPosition;
            item.id = BuildingsGO[item.id].GetComponent<DraggableBuildings>().id;
            item.buildingID = BuildingsGO[item.id].GetComponent<DraggableBuildings>().buildingID;
            item.level = BuildingsGO[item.id].GetComponent<DraggableBuildings>().level;
            item.state = (sbyte)BuildingsGO[item.id].GetComponent<DraggableBuildings>().state;
            item.unlockedQueueSlots = BuildingsGO[item.id].GetComponent<DraggableBuildings>().unlockedQueueSlots;
            item.itemID = BuildingsGO[item.id].GetComponent<DraggableBuildings>().itemID;
            item.dateTime = BuildingsGO[item.id].GetComponent<DraggableBuildings>().dateTime.ToString();
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
}

;

[System.Serializable]
public class AAA  // iLIST
{
    public int id;
    public string name;
    public Queue<int> aa;

    public AAA()
    {

    }

    public AAA(int _id, string _name, Queue<int> _aa)
    {
        id = _id;
        name = _name;
        aa = _aa;
    }
}