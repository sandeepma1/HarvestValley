using System;
using System.Collections.Generic;
using UnityEngine;
using HarvestValley.Ui;

namespace HarvestValley.Managers
{
    public class BuildingManager : ManagerBase<BuildingManager>
    {
        private List<Buildings> buildings = new List<Buildings>();
        [SerializeField]
        private ClickableBuilding buildingPrefab;
        public ClickableBuilding[] BuildingsGO;

        private void Start()
        {
            ClickableBuilding.OnBuildingClicked += OnBuildingClickedEventHandler;
            OneTimeOnly();
            Init();
            //ToggleFieldSelector(false);
        }

        public override void OnBuildingClickedEventHandler(int buildingID, int sourceID)
        {
            base.OnBuildingClickedEventHandler(buildingID, sourceID);
            MenuManager.Instance.DisplayMenu(MenuNames.BuildingMenu, MenuOpeningType.CloseAll);

            switch (BuildingsGO[buildingID].state)
            {
                case BuildingState.IDLE:
                case BuildingState.WORKING:
                    MenuManager.Instance.DisplayMenu(MenuNames.BuildingMenu, MenuOpeningType.CloseAll);
                    break;
                case BuildingState.DONE:
                    MenuManager.Instance.CloseAllMenu();
                    //CollectItemsOnFields(buildingID); // TODO: Directly collect/harvest on feild click
                    break;
                default:
                    break;
            }

        }

        #region Creating buildings from save
        private void Init()
        {
            buildings = ES2.LoadList<Buildings>("AllBuildings");
            BuildingsGO = new ClickableBuilding[buildings.Count];
            for (int i = 0; i < buildings.Count; i++)
            {
                InitBuildings(buildings[i]);
            }
            InvokeRepeating("SaveBuildings", 0, 5);
            //InvokeRepeating("CheckForHarvest", 0, 1);
        }

        private void InitBuildings(Buildings building)
        {
            BuildingsGO[building.id] = Instantiate(buildingPrefab, transform);
            BuildingsGO[building.id].gameObject.name = "Building" + building.id;
            BuildingsGO[building.id].buildingSprite.sprite = AtlasBank.Instance.GetSprite(SourceDatabase.Instance.sources[building.buildingID].slug, AtlasType.Buildings);
            BuildingsGO[building.id].buildingID = building.id;
            BuildingsGO[building.id].sourceID = building.buildingID;
            BuildingsGO[building.id].itemID = building.itemID;
            BuildingsGO[building.id].state = (BuildingState)building.state;
            BuildingsGO[building.id].dateTime = DateTime.Parse(building.dateTime);
            //CalculateFeildCrop(buildingsGO[building.id]);
        }
        #endregion

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

        private void SaveBuildings()
        {
            foreach (var item in buildings)
            {
                //item.pos = buildingsGO[item.id].transform.localPosition;
                item.id = BuildingsGO[item.id].buildingID;
                item.buildingID = BuildingsGO[item.id].sourceID;
                //item.level = buildingsGO[item.id].level;
                item.state = (sbyte)BuildingsGO[item.id].state;
                //item.unlockedQueueSlots = buildingsGO[item.id].unlockedQueueSlots;
                item.itemID = BuildingsGO[item.id].itemID;
                item.dateTime = BuildingsGO[item.id].dateTime.ToString();
            }
            ES2.Save(buildings, "AllBuildings");
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

    public Buildings(int f_id, int f_buildingID, string f_name, int f_state, int f_itemID, string f_dateTime)//, Queue <int>  f_itemID, Queue <string>  f_dateTime)
    {
        id = f_id;
        buildingID = f_buildingID;
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
