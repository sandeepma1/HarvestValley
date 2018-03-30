using System;
using System.Collections.Generic;
using UnityEngine;
using HarvestValley.Ui;
using HarvestValley.IO;

namespace HarvestValley.Managers
{
    public class BuildingManager : ManagerBase<BuildingManager>
    {
        [SerializeField]
        private ClickableBuilding buildingPrefab;

        public ClickableBuilding[] BuildingsGO;
        private List<Buildings> buildings = new List<Buildings>();

        private void Start()
        {
            Init();
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
        }

        private void InitBuildings(Buildings building)
        {
            BuildingsGO[building.id] = Instantiate(buildingPrefab, transform);
            BuildingsGO[building.id].gameObject.name = "Building" + building.id;
            BuildingsGO[building.id].buildingSprite.sprite = AtlasBank.Instance.GetSprite(SourceDatabase.GetSourceInfoById(building.buildingID).slug, AtlasType.Buildings);
            BuildingsGO[building.id].buildingId = building.id;
            BuildingsGO[building.id].sourceId = building.buildingID;

            BuildingsGO[building.id].PopulateBuildingQueueFromSave(building.itemID, building.dateTime);
            BuildingsGO[building.id].unlockedQueueSlots = building.unlockedQueueSlots;

            BuildingsGO[building.id].state = (BuildingState)building.state;
        }
        #endregion

        public override void OnBuildingClicked(int buildingID, int sourceID)
        {
            base.OnBuildingClicked(buildingID, sourceID);
            // MenuManager.Instance.DisplayMenu(MenuNames.BuildingMenu, MenuOpeningType.CloseAll);

            switch (BuildingsGO[buildingID].state)
            {
                case BuildingState.IDLE:
                case BuildingState.WORKING:
                    MenuManager.Instance.DisplayMenu(MenuNames.BuildingMenu, MenuOpeningType.CloseAll);
                    UiBuildingMenu.Instance.EnableMenu();
                    break;
                case BuildingState.DONE:
                    //MenuManager.Instance.CloseAllMenu();
                    //CollectItemsOnFields(buildingID); // TODO: Directly collect/harvest on feild click
                    break;
                default:
                    break;
            }

        }

        public void SaveBuildings()
        {
            foreach (var item in buildings)
            {
                //item.pos = buildingsGO[item.id].transform.localPosition;
                item.id = BuildingsGO[item.id].buildingId;
                item.buildingID = BuildingsGO[item.id].sourceId;
                //item.level = buildingsGO[item.id].level;
                item.state = (sbyte)BuildingsGO[item.id].state;
                item.unlockedQueueSlots = BuildingsGO[item.id].unlockedQueueSlots;

                BuildingQueue[] currentQueue = BuildingsGO[item.id].CurrentItemsInQueue();

                for (int i = 0; i < currentQueue.Length; i++)
                {
                    item.itemID[i] = currentQueue[i].itemId;
                    item.dateTime[i] = currentQueue[i].dateTime.ToString();
                }

                for (int i = currentQueue.Length; i < GEM.maxBuildingQueueCount; i++)
                {
                    item.itemID[i] = -1;
                    item.dateTime[i] = DateTime.UtcNow.ToString();
                }
            }
            ES2.Save(buildings, "AllBuildings");
        }
    }
}

[Serializable]
public class Buildings  // iLIST
{
    public int id;
    public int buildingID;
    public string name;
    public int state;
    public int unlockedQueueSlots;
    public int[] itemID;
    public string[] dateTime;

    public Buildings()
    {
    }

    public Buildings(int f_id, int f_buildingID, string f_name, int f_state, int f_unlockedQueueSlots, int[] f_itemID, string[] f_dateTime)//, Queue <int>  f_itemID, Queue <string>  f_dateTime)
    {
        id = f_id;
        buildingID = f_buildingID;
        name = f_name;
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

[Serializable]
public struct BuildingQueue
{
    public int itemId;
    public DateTime dateTime;
}