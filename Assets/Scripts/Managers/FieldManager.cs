﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using HarvestValley.Ui;

namespace HarvestValley.Managers
{
    public class FieldManager : ManagerBase<FieldManager>
    {
        [SerializeField]
        private Transform fieldSelector;
        [SerializeField]
        private ClickableField fieldPrefab;

        public bool plantedOnSelectedfield = false;
        public int fieldSelectedID = -1;
        public ClickableField[] FieldGO;
        public int itemSelectedID = -1; // TODO: delete this asap

        private List<Fields> fields = new List<Fields>();

        private void Start()
        {
            //ClickableField.OnBuildingClicked += OnBuildingClickedEventHandler;
            OneTimeOnly();
            Init();
            ToggleFieldSelector(false);
        }

        private void Init()
        {
            fields = ES2.LoadList<Fields>("AllFields");
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
            if (PlayerPrefs.GetInt("firstField") == 0)
            {
                ES2.Delete("AllFields");
                int counter = 0;
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        fields.Add(new Fields(counter, 0, "Field", new Vector2(i * gap, -j * gap), 1, 0, -1, System.DateTime.UtcNow.ToString()));
                        counter++;
                    }
                }
                ES2.Save(fields, "AllFields");
                PlayerPrefs.SetInt("firstField", 1);
            }
        }

        private void InitFields(Fields field)
        {
            FieldGO[field.id] = Instantiate(fieldPrefab, transform);
            FieldGO[field.id].transform.localPosition = field.pos;
            FieldGO[field.id].gameObject.name = "Field" + field.id;
            FieldGO[field.id].buildingSprite.sprite = AtlasBank.Instance.GetSprite(SourceDatabase.Instance.sources[field.fieldID].slug, AtlasType.Buildings);
            FieldGO[field.id].buildingId = field.id;
            FieldGO[field.id].sourceId = field.fieldID;
            FieldGO[field.id].pos = field.pos;
            FieldGO[field.id].level = field.level;
            FieldGO[field.id].itemId = field.itemID;
            FieldGO[field.id].state = (BuildingState)field.state;
            FieldGO[field.id].dateTime = DateTime.Parse(field.dateTime);
            CalculateFeildCrop(FieldGO[field.id]);
        }

        private void CheckForHarvest()
        {
            for (int i = 0; i < FieldGO.Length; i++)
            {
                if (FieldGO[i] == null)
                    return;

                switch (FieldGO[i].state)
                {
                    case BuildingState.IDLE:
                        break;
                    case BuildingState.WORKING:
                        CalculateFeildCrop(FieldGO[i]);
                        break;
                    case BuildingState.DONE:
                        break;
                    default:
                        break;
                }
            }
        }

        private void CalculateFeildCrop(ClickableField field)
        {
            if (field.itemId < 0)
            {
                return;
            }

            TimeSpan timeElapsed = field.dateTime - DateTime.UtcNow;
            float timeElapsedInSeconds = (float)timeElapsed.TotalSeconds;
            float divisionFactor = (ItemDatabase.Instance.items[field.sourceId].timeRequiredInMins * 60) / 4;

            if (timeElapsedInSeconds >= divisionFactor * 3) //22.5 seed
            {
                ChangeFarmPlantSprite(field, PlantStage.SEED);
            }
            else if (timeElapsedInSeconds >= divisionFactor * 2) //15 shrub
            {
                ChangeFarmPlantSprite(field, PlantStage.SHRUB);
            }
            else if (timeElapsedInSeconds >= divisionFactor) //7.5 plant
            {
                ChangeFarmPlantSprite(field, PlantStage.PLANT);
            }
            else if (timeElapsedInSeconds <= 0) // 0 mature
            {
                ChangeFarmPlantSprite(field, PlantStage.MATURE);
                field.state = BuildingState.DONE;
                field.dateTime = new System.DateTime();
            }
        }

        private void ChangeFarmPlantSprite(ClickableField field, PlantStage stages)
        {
            string itemSlug = ItemDatabase.Instance.items[field.itemId].slug;
            switch (stages)
            {
                case PlantStage.SEED:
                    if (field.plantsSprite.sprite != AtlasBank.Instance.GetSprite(itemSlug + "_0", AtlasType.Farming))
                    {
                        field.plantsSprite.sprite = AtlasBank.Instance.GetSprite(itemSlug + "_0", AtlasType.Farming);
                    }
                    break;
                case PlantStage.SHRUB:
                    if (field.plantsSprite.sprite != AtlasBank.Instance.GetSprite(itemSlug + "_1", AtlasType.Farming))
                    {
                        field.plantsSprite.sprite = AtlasBank.Instance.GetSprite(itemSlug + "_1", AtlasType.Farming);
                    }
                    break;
                case PlantStage.PLANT:
                    if (field.plantsSprite.sprite != AtlasBank.Instance.GetSprite(itemSlug + "_2", AtlasType.Farming))
                    {
                        field.plantsSprite.sprite = AtlasBank.Instance.GetSprite(itemSlug + "_2", AtlasType.Farming);
                    }
                    break;
                case PlantStage.MATURE:
                    if (field.plantsSprite.sprite != AtlasBank.Instance.GetSprite(itemSlug + "_3", AtlasType.Farming))
                    {
                        field.plantsSprite.sprite = AtlasBank.Instance.GetSprite(itemSlug + "_3", AtlasType.Farming);
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
                if (FieldGO[i].state == BuildingState.IDLE)
                {
                    FieldGO[i].StartPlantingMode(itemID);
                }
            }
            SaveFields();
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

        public void CollectItemsOnFields(int fieldID) //Collecting Items on fields
        {
            PlayerInventoryManager.Instance.UpdateFarmItems(FieldGO[fieldID].itemId, 1);
            PlayerProfileManager.Instance.PlayerXPPointsAdd(ItemDatabase.Instance.items[FieldGO[fieldID].itemId].XPperYield);
            FieldGO[fieldID].state = BuildingState.IDLE;
            FieldGO[fieldID].dateTime = new System.DateTime();
            FieldGO[fieldID].itemId = -1;
            FieldGO[fieldID].plantsSprite.sprite = new Sprite();
        }

        public void AddNewField(Vector2 pos, int fieldID)
        {
            fields.Add(new Fields(fields.Count + 1, fieldID, SourceDatabase.Instance.sources[fieldID].sourceID.ToString(), pos,
                1, 0, -1, System.DateTime.UtcNow.ToString()));
            ES2.Save(fields, "AllFields");
            InitFields(fields[fields.Count - 1]);
        }

        #region Field Selector

        private void SelectField()
        {
            if (FieldGO[currentSelectedBuildingID].state == BuildingState.IDLE ||
                FieldGO[currentSelectedBuildingID].state == BuildingState.WORKING)
            {
                ToggleFieldSelector(true);
                fieldSelector.SetParent(FieldGO[currentSelectedBuildingID].transform);
                fieldSelector.transform.localPosition = Vector3.zero;
                fieldSelectedID = currentSelectedBuildingID;
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

        internal void DeselectField()
        {
            if (currentSelectedBuildingID == -1)
            {
                return;
            }
            ToggleFieldSelector(false);
            currentSelectedBuildingID = -1;
        }

        #endregion 

        #region OnMouse Functions

        public override void OnBuildingClicked(int buildingID, int sourceID)
        {
            base.OnBuildingClicked(buildingID, sourceID);
            SelectField();
            switch (FieldGO[buildingID].state)
            {
                case BuildingState.IDLE:
                    MenuManager.Instance.DisplayMenu(MenuNames.SeedList, MenuOpeningType.CloseAll);
                    MenuManager.Instance.DisplayMenu(MenuNames.FieldUpgrade, MenuOpeningType.OnTop);
                    break;
                case BuildingState.WORKING:
                    MenuManager.Instance.DisplayMenu(MenuNames.FieldProgress, MenuOpeningType.CloseAll);
                    MenuManager.Instance.DisplayMenu(MenuNames.FieldUpgrade, MenuOpeningType.OnTop);
                    break;
                case BuildingState.DONE:
                    MenuManager.Instance.CloseAllMenu();
                    CollectItemsOnFields(buildingID); // Directly collect/harvest on feild click
                    break;
                default:
                    break;
            }
        }

        #endregion

        public void SaveFields()
        {
            foreach (var item in fields)
            {
                item.pos = FieldGO[item.id].transform.localPosition;
                item.id = FieldGO[item.id].buildingId;
                item.fieldID = FieldGO[item.id].sourceId;
                item.level = FieldGO[item.id].level;
                item.state = (sbyte)FieldGO[item.id].state;
                item.itemID = FieldGO[item.id].itemId;
                item.dateTime = FieldGO[item.id].dateTime.ToString();
            }
            ES2.Save(fields, "AllFields");
        }

        IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("Main");
        }
    }
}

[System.Serializable]
public class Fields  // iLIST
{
    public int id;
    public int fieldID;
    public string name;
    public Vector2 pos;
    public int level;
    public int state;
    public int itemID;
    public string dateTime;

    public Fields()
    {
    }

    public Fields(int f_id, int f_fieldID, string f_name, Vector2 f_pos, int f_level, int f_state, int f_itemID, string f_dateTime)//, Queue <int>  f_itemID, Queue <string>  f_dateTime)
    {
        id = f_id;
        fieldID = f_fieldID;
        name = f_name;
        pos = f_pos;
        level = f_level;
        state = f_state;
        itemID = f_itemID;
        dateTime = f_dateTime;
    }
}

public enum PlantStage
{
    SEED,
    SHRUB,
    PLANT,
    MATURE
};