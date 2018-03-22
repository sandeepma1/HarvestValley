﻿using System;
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
            //ClickableBuilding.OnBuildingClicked += OnBuildingClickedEventHandler;
            OneTimeOnly();
            Init();
            //ToggleFieldSelector(false);
        }

        public override void OnBuildingClicked(int buildingID, int sourceID)
        {
            base.OnBuildingClicked(buildingID, sourceID);
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
            BuildingsGO[building.id].buildingId = building.id;
            BuildingsGO[building.id].sourceId = building.buildingID;

            BuildingsGO[building.id].PopulateBuildingQueueFromSave(building.itemID, building.dateTime);
            BuildingsGO[building.id].unlockedQueueSlots = building.unlockedQueueSlots;

            BuildingsGO[building.id].state = (BuildingState)building.state;

            //BuildingsGO[building.id].dateTime = new DateTime[GEM.maxQCount];

            //for (int i = 0; i < GEM.maxQCount; i++)
            //{
            //    BuildingsGO[building.id].dateTime[i] = DateTime.Parse(building.dateTime[i]);
            //}

            //CalculateFeildCrop(buildingsGO[building.id]);
        }
        #endregion

        public void ItemDroppedInZone(int itemId)
        {
            BuildingsGO[currentSelectedBuildingID].AddToProductionQueue(itemId);
        }

        private void OneTimeOnly()
        {
            if (PlayerPrefs.GetInt("firstBuilding") == 0)
            {
                ES2.Delete("AllBuildings");

                string[] nowTime = new string[GEM.maxQCount];
                int[] ids = new int[GEM.maxQCount];
                for (int i = 0; i < GEM.maxQCount; i++)
                {
                    nowTime[i] = DateTime.UtcNow.ToString();
                    ids[i] = -1;
                }

                buildings.Add(new Buildings(0, 2, "Building", 0, 2, ids, nowTime));

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
                item.id = BuildingsGO[item.id].buildingId;
                item.buildingID = BuildingsGO[item.id].sourceId;
                //item.level = buildingsGO[item.id].level;
                item.state = (sbyte)BuildingsGO[item.id].state;
                item.unlockedQueueSlots = BuildingsGO[item.id].unlockedQueueSlots;

                BuildingQueue[] currentQueue = BuildingsGO[item.id].CurrentItemsInQueue();

                for (int i = 0; i < currentQueue.Length; i++)
                {
                    item.itemID[i] = currentQueue[i].id;
                    item.dateTime[i] = currentQueue[i].dateTime.ToString();
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
    public int id;
    public DateTime dateTime;
}