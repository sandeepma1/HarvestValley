using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    public static GrassManager Instance = null;
    public DraggableGrass grassPrefab;
    public GameObject MasterMenuGO = null;
    public List<GrassTypes> grassSaved = new List<GrassTypes>();
    public int buildingSelectedID = -1;
    public DraggableGrass[] grassPatches;
    TimeSpan remainingTime;
    int tempID = -1, mouseDownBuildingID = -1;
    bool isTilePressed = false;
    public Sprite[] grassSpriteBank;
    private Sprite[] buildingSpriteBank;

    private void Awake()
    {
        Instance = this;
        OneTimeOnly();
        //https://answers.unity.com/questions/1175266/getting-single-sprite-from-a-sprite-multiple.html
        grassSpriteBank = Resources.LoadAll<Sprite>("Textures/Plants"); // loads all sprite from Resource folder
        buildingSpriteBank = Resources.LoadAll<Sprite>("Textures/Buildings");
        Init();
    }

    private void Init()
    {
        grassSaved = ES2.LoadList<GrassTypes>("AllGrass");
        //BuildingsGO = new GameObject[buildings.Count];
        grassPatches = new DraggableGrass[99];
        foreach (var grass in grassSaved)
        {
            InitBuildings(grass);
        }
        //InvokeRepeating("SaveGrassPatch", 0, 5);
        InvokeRepeating("CheckForHarvest", 0, 1);
    }

    public void InitBuildings(GrassTypes grass)
    {
        grassPatches[grass.id] = Instantiate(grassPrefab, transform);
        grassPatches[grass.id].gameObject.name = "GrassPatch" + grass.id;
        grassPatches[grass.id].id = grass.id;
        grassPatches[grass.id].grassTypeID = grass.grassTypeID;
        grassPatches[grass.id].state = (BUILDINGS_STATE)grass.state;

        switch (grassPatches[grass.id].state)
        {
            case BUILDINGS_STATE.NONE:
                //BuildingsGO[building.id].spriteRenderer.color = Color.white;
                break;
            case BUILDINGS_STATE.GROWING:
                //BuildingsGO[building.id].spriteRenderer.color = Color.green;
                break;
            case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                //BuildingsGO[building.id].spriteRenderer.color = Color.red;
                break;
            default:
                break;
        }
        grassPatches[grass.id].dateTime = DateTime.Parse(grass.dateTime);
    }

    private void Update() // all long press logic	
    {
        //if (isTilePressed && grassPatches[mouseDownBuildingID].buildingID != 0)
        //{
        //    if (longPressTimer >= longPressTime)
        //    {
        //        isLongPress = true;
        //        longPressBuildingID = mouseDownBuildingID;
        //        mouseDownBuildingID = -1;
        //        isTilePressed = false;
        //        grassPatches[longPressBuildingID].isSelected = true;
        //        EnableOutlineOnSprite(longPressBuildingID);
        //        return;
        //    }
        //    longPressTimer += Time.deltaTime;
        //}
    }

    private void LateUpdate() //Mainly used to show time remaining
    {
        //if (isFarmTimerEnabled)
        //{
        //    ShowFarmLandTimeRemaining();
        //} else
        //{
        //    tempID = -1;
        //}
    }

    private void CheckForHarvest()
    {
        for (int i = 0; i < grassPatches.Length; i++)
        {
            if (grassPatches[i] != null && grassPatches[i].state == BUILDINGS_STATE.GROWING)
            {
                TimeSpan currentTime = grassPatches[i].dateTime.Subtract(UTC.time.liveDateTime);
                float currentTimeInSeconds = currentTime.Seconds;
                float totalSeconds = ItemDatabase.Instance.items[grassPatches[i].grassTypeID].timeRequiredInMins * 60;
                float divisionFactor = totalSeconds / 4;

                if (currentTimeInSeconds >= divisionFactor * 3) //22.5 seed
                {
                    ChangeFarmPlantSprite(grassPatches[i], PLANT_STAGES.SEED);
                } else if (currentTimeInSeconds >= divisionFactor * 2) //15 shrub
                {
                    ChangeFarmPlantSprite(grassPatches[i], PLANT_STAGES.SHRUB);
                } else if (currentTimeInSeconds >= divisionFactor) //7.5 plant
                {
                    ChangeFarmPlantSprite(grassPatches[i], PLANT_STAGES.PLANT);
                } else if (currentTimeInSeconds <= 0) // 0 mature
                {
                    ChangeFarmPlantSprite(grassPatches[i], PLANT_STAGES.MATURE);
                    grassPatches[i].state = BUILDINGS_STATE.WAITING_FOR_HARVEST;
                    grassPatches[i].dateTime = new System.DateTime();
                    //BuildingsGO[i].spriteRenderer.color = Color.red;
                }
            }
        }
    }

    private void ChangeFarmPlantSprite(DraggableGrass building, PLANT_STAGES stages)
    {
        //switch (stages)
        //{
        //    case PLANT_STAGES.SEED:
        //        building.grassSprite.sprite = GetPlantSpriteFromBank(ItemDatabase.Instance.items[building.grassTypeID].name + "_0");
        //        break;
        //    case PLANT_STAGES.SHRUB:
        //        building.grassSprite.sprite = GetPlantSpriteFromBank(ItemDatabase.Instance.items[building.grassTypeID].name + "_1");
        //        break;
        //    case PLANT_STAGES.PLANT:
        //        building.grassSprite.sprite = GetPlantSpriteFromBank(ItemDatabase.Instance.items[building.grassTypeID].name + "_2");
        //        break;
        //    case PLANT_STAGES.MATURE:
        //        building.grassSprite.sprite = GetPlantSpriteFromBank(ItemDatabase.Instance.items[building.grassTypeID].name + "_3");
        //        break;
        //    default:
        //        break;
        //}
    }

    private Sprite GetPlantSpriteFromBank(string spriteName)
    {
        Sprite sprite = new Sprite();
        sprite = grassSpriteBank.Single(s => s.name == spriteName);
        if (sprite != null)
        {
            return grassSpriteBank.Single(s => s.name == spriteName);
        } else
        {
            Debug.Log("Sprite Not Found " + spriteName);
            return new Sprite();
        }
    }

    public void DisplayMasterMenu(int b_ID) // Display field Crop Menu
    {
        MasterMenuManager.Instance.PopulateItemsInMasterMenu(grassPatches[b_ID].grassTypeID);
        MenuManager.Instance.DisableAllMenus();
        buildingSelectedID = b_ID;

        //Animatin Stuff
        // MasterMenuGO.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        MasterMenuGO.transform.position = grassPatches[b_ID].transform.position;
        MasterMenuGO.SetActive(true);
        //LeanTween.scale(MasterMenuGO.transform.GetChild(0).gameObject, new Vector3(2, 2, 2), 0.2f, IGMMenu.m_instance.ease);
    }

    public void PlantItemsOnBuildings(int buildingID) // Planting Items
    {
        if (MasterMenuManager.Instance.isItemSelected == true)
        {
            if (grassPatches[buildingID].grassTypeID == 0)
            { // selected building is feild
                if (buildingSelectedID == buildingID)
                {
                    if (PlayerInventoryManager.Instance.playerInventory[MasterMenuManager.Instance.itemSelectedID].count >= 1)
                    {
                        grassPatches[buildingID].state = BUILDINGS_STATE.GROWING;
                        grassPatches[buildingID].grassTypeID = MasterMenuManager.Instance.itemSelectedID;
                        grassPatches[buildingID].dateTime = UTC.time.liveDateTime.AddMinutes(
                            ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].timeRequiredInMins);
                        // BuildingsGO[buildingID].spriteRenderer.color = Color.green;

                        string plantName = ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].name + "_0";

                        grassPatches[buildingID].grassSprite.sprite = GetPlantSpriteFromBank(plantName);
                        PlayerInventoryManager.Instance.playerInventory[MasterMenuManager.Instance.itemSelectedID].count--;
                        MasterMenuManager.Instance.UpdateSeedValue();
                        SaveGrassPatch();
                        buildingSelectedID = -1;
                    }
                }
            } else
            { // if selected building is NOT feild
                if (buildingSelectedID == buildingID && DoesInventoryHasItems(buildingID))
                {
                    DecrementItemsFromInventory();
                    grassPatches[buildingID].state = BUILDINGS_STATE.GROWING;
                    grassPatches[buildingID].grassTypeID = MasterMenuManager.Instance.itemSelectedID;
                    grassPatches[buildingID].dateTime = UTC.time.liveDateTime.AddMinutes(
                        ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].timeRequiredInMins);
                    grassPatches[buildingID].grassSprite.color = Color.green;
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

        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID1 >= 0)
        {
            if (PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID1].count >=
                ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needAmount1)
            {
                needItems1 = 0;
                print("1 ok");
            } else
            {
                needItems1 = -2;
            }
        }
        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID2 >= 0)
        {
            if (PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID2].count >=
                ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needAmount2)
            {
                needItems2 = 0;
                print("1 ok");
            } else
            {
                needItems2 = -2;
            }
        }
        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID3 >= 0)
        {
            if (PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID3].count >=
                ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needAmount3)
            {
                needItems3 = 0;
                print("1 ok");
            } else
            {
                needItems3 = -2;
            }
        }
        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID4 >= 0)
        {
            if (PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID4].count >=
                ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needAmount4)
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
        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID1 >= 0)
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID1].count =
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID1].count -
            ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needAmount1;

        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID2 >= 0)
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID2].count =
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID2].count -
            ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needAmount2;

        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID3 >= 0)
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID3].count =
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID3].count -
            ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needAmount3;

        if (ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID4 >= 0)
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID4].count =
            PlayerInventoryManager.Instance.playerInventory[ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needID4].count -
            ItemDatabase.Instance.items[MasterMenuManager.Instance.itemSelectedID].needAmount4;
    }

    public void HarvestCropOnFarmLand(int buildingID) // Harvesting Seeds calls only on farms
    {
        if (HarvestMenuManager.Instance.isScytheSelected == true)
        {
            //// TODO Heavy update required for field Level Based cals*******************
            //// only 2 items are added in storage
            ////			print (FarmLands [buildingID].GetComponent <FarmLands> ().itemID);		 
            //PlayerInventoryManager.Instance.UpdateFarmItems(Convert.ToInt32(grassPatches[buildingID].itemID), 2);
            //PlayerProfileManager.Instance.PlayerXPPointsAdd(ItemDatabase.Instance.items[grassPatches[buildingID].itemID].XP);
            //grassPatches[buildingID].state = BUILDINGS_STATE.NONE;

            //grassPatches[buildingID].dateTime = new System.DateTime();
            //grassPatches[buildingID].itemID = -1;
            ////BuildingsGO[buildingID].spriteRenderer.color = Color.white;
            //grassPatches[buildingID].plantsSprite.sprite = new Sprite();
            //HarvestMenuManager.Instance.ToggleDisplayHarvestingMenu();
        }
    }

    public void ShowReadyToHarvestCropsMenu(int buildingID) // Display Harvesting Menu
    {
        //MenuManager.Instance.DisableAllMenus();
        //FarmHarvestingMenu.transform.position = grassPatches[buildingID].transform.position;
        //FarmHarvestingMenu.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        //FarmHarvestingMenu.SetActive(true);
        //LeanTween.scale(FarmHarvestingMenu, Vector3.one, 0.2f, MenuManager.Instance.ease);
    }

    public void CollectItemsOnBuildings(int buildingID) //Collecting Items on buildings
    {
        PlayerInventoryManager.Instance.UpdateFarmItems(grassPatches[buildingID].grassTypeID, 1);
        PlayerProfileManager.Instance.PlayerXPPointsAdd(ItemDatabase.Instance.items[grassPatches[buildingID].grassTypeID].XP);
        grassPatches[buildingID].state = BUILDINGS_STATE.NONE;
        grassPatches[buildingID].dateTime = new System.DateTime();
        grassPatches[buildingID].grassTypeID = -1;
        // BuildingsGO[buildingID].spriteRenderer.color = Color.white;
        grassPatches[buildingID].grassSprite.sprite = new Sprite();
    }

    public void DisableAnyOpenMenus()
    {
        //for (int i = 0; i < grassPatches.Length; i++)
        //{
        //    if (grassPatches[i] != null)
        //    {
        //        grassPatches[i].isSelected = false;
        //        DisableOutlineOnSprite(i);
        //    }
        //}
        //isLongPress = false;
    }

    public void ShowFarmLandTimeRemaining()
    {
        //remainingTime = grassPatches[tempID].dateTime.Subtract(UTC.time.liveDateTime);
        //TimeRemainingMenu.transform.position = grassPatches[tempID].transform.position;
        //if (remainingTime <= new System.TimeSpan(360, 0, 0, 0))
        //{ //> 1year
        //    TimeRemainingMenu.transform.GetChild(1).GetComponent<TextMeshPro>().text = remainingTime.Days.ToString() + "d " + remainingTime.Hours.ToString() + "h";
        //}
        //if (remainingTime <= new System.TimeSpan(1, 0, 0, 0))
        //{ //> 1day
        //    TimeRemainingMenu.transform.GetChild(1).GetComponent<TextMeshPro>().text = remainingTime.Hours.ToString() + "h " + remainingTime.Minutes.ToString() + "m";
        //}
        //if (remainingTime <= new System.TimeSpan(0, 1, 0, 0))
        //{ //> 1hr
        //    TimeRemainingMenu.transform.GetChild(1).GetComponent<TextMeshPro>().text = remainingTime.Minutes.ToString() + "m " + remainingTime.Seconds.ToString() + "s";
        //}
        //if (remainingTime <= new System.TimeSpan(0, 0, 1, 0))
        //{ // 1min
        //    TimeRemainingMenu.transform.GetChild(1).GetComponent<TextMeshPro>().text = remainingTime.Seconds.ToString() + "s";
        //}
        //if (remainingTime <= new System.TimeSpan(0, 0, 0, 0))
        //{ // 1min
        //    TimeRemainingMenu.SetActive(false);
        //}
    }

    //private void EnableOutlineOnSprite(int selectedFieldID)
    //{
    //    if (grassPatches[selectedFieldID].buildingID != 0)
    //    {
    //        grassPatches[selectedFieldID].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    //    }

    //}

    //private void DisableOutlineOnSprite(int selectedFieldID)
    //{
    //    if (grassPatches[selectedFieldID].buildingID != 0)
    //    {
    //        grassPatches[selectedFieldID].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
    //    }
    //}

    public void AddNewBuilding(Vector2 pos, int buildingID)
    {
        //grassSaved.Add(new Buildings(grassSaved.Count + 1, buildingID, BuildingDatabase.Instance.buildingInfo[buildingID].name.ToString(), pos,
        //    1, 0, 0, -1, System.DateTime.UtcNow.ToString()));
        //ES2.Save(grassSaved, "AllBuildings");
        //InitBuildings(grassSaved[grassSaved.Count - 1]);
    }

    private void OneTimeOnly()
    {
        if (PlayerPrefs.GetInt("firstGrass") <= 0)
        {
            ES2.Delete("AllGrass");
            grassSaved.Add(new GrassTypes(0, 0, 0, System.DateTime.UtcNow.ToString()));
            grassSaved.Add(new GrassTypes(1, 0, 0, System.DateTime.UtcNow.ToString()));
            grassSaved.Add(new GrassTypes(2, 0, 0, System.DateTime.UtcNow.ToString()));
            grassSaved.Add(new GrassTypes(3, 0, 0, System.DateTime.UtcNow.ToString()));
            grassSaved.Add(new GrassTypes(4, 0, 0, System.DateTime.UtcNow.ToString()));
            ES2.Save(grassSaved, "AllGrass");
            PlayerPrefs.SetInt("firstGrass", 1);
        }
    }

    #region OnMouse Functions

    public void CallParentOnMouseDown(int buildingID)
    {
        isTilePressed = true;
        // longPressTimer = 0;
        mouseDownBuildingID = buildingID;
        //if (buildingID != longPressBuildingID && longPressBuildingID != -1)
        //{
        //    grassPatches[longPressBuildingID].isSelected = false;
        //    DisableOutlineOnSprite(longPressBuildingID);
        //    isLongPress = false;
        //}
    }

    public void CallParentOnMouseUp(int buildingID)
    {
        isTilePressed = false;
        mouseDownBuildingID = -1;

        switch (grassPatches[buildingID].state)
        {
            case BUILDINGS_STATE.NONE:
                if (GEM.GetTouchState() == GEM.TOUCH_STATES.e_none)
                {
                    DisplayMasterMenu(buildingID);
                }
                break;
            case BUILDINGS_STATE.GROWING:
                tempID = buildingID;
                MenuManager.Instance.DisableAllMenus();
                if (GEM.GetTouchState() == GEM.TOUCH_STATES.e_none)
                {

                }
                break;
            case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                if (grassPatches[buildingID].grassTypeID == 0)
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

    }

    public void CallParentOnMouseEnter(int buildingID)
    {
        switch (grassPatches[buildingID].state)
        {
            case BUILDINGS_STATE.NONE:
                PlantItemsOnBuildings(buildingID);
                break;
            case BUILDINGS_STATE.WAITING_FOR_HARVEST:
                if (grassPatches[buildingID].grassTypeID == 0)
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
        if (grassPatches[buildingID].isSelected)
        {
            grassPatches[buildingID].transform.position = new Vector3(Mathf.Round(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0)).x),
                Mathf.Round(Camera.main.ScreenToWorldPoint(new Vector3(0, Input.mousePosition.y, 0)).y), 0);
        }
    }

    #endregion

    void SaveGrassPatch()
    {
        foreach (var item in grassSaved)
        {
            item.id = grassPatches[item.id].id;
            item.state = (sbyte)grassPatches[item.id].state;
            item.grassTypeID = grassPatches[item.id].grassTypeID;
            item.dateTime = grassPatches[item.id].dateTime.ToString();
        }
        ES2.Save(grassSaved, "AllGrass");
    }
}

[System.Serializable]
public class GrassTypes  // iLIST
{
    public int id;
    public int state;
    public int grassTypeID;
    public string dateTime;

    public GrassTypes()
    {
    }

    public GrassTypes(int f_id, int f_grassID, int f_state, string f_dateTime)
    {
        id = f_id;
        grassTypeID = f_grassID;
        state = f_state;
        dateTime = f_dateTime;
    }
}

public enum GRASS_STATE
{
    NONE,
    GROWING,
    WAITING_FOR_HARVEST
};


