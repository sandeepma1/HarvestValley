using System.Collections.Generic;
using UnityEngine;
using HarvestValley.Ui;
using System;

namespace HarvestValley.Managers
{
    public class GrassLandManager : ManagerBase<GrassLandManager>
    {
        [SerializeField]
        private ClickableGrass clickableGrassPrefab;
        [SerializeField]
        private int x = 12, y = 12;
        public bool isinPlantingMode = false;

        private ClickableGrass[] grassGO;
        public List<Grass> grass = new List<Grass>();
        public int selectedItemIdInMenu;

        public InventoryItems[] grassItemDatabase;

        private void Start()
        {
            grass = ES2.LoadList<Grass>("AllGrass");
            grassGO = new ClickableGrass[grass.Count];

            for (int i = 0; i < grass.Count; i++)
            {
                InitGrassPatches(grass[i]);
            }
        }

        private void InitGrassPatches(Grass grass)
        {
            grassGO[grass.grassId] = Instantiate(clickableGrassPrefab, transform);
            grassGO[grass.grassId].grass = grass;
            grassGO[grass.grassId].transform.localPosition = grass.position;
            grassGO[grass.grassId].gameObject.name = "Grass" + grass.grassId;
            grassGO[grass.grassId].OnClickOpenMenu += ClickableGrasssClickedEventHandler;
            grassGO[grass.grassId].ClickableGrassAddedItem += ClickableGrassAddedItemEventHandler;
        }

        //Todo: Need to implement grass's all items and counts
        private void ClickableGrassAddedItemEventHandler(int itemId)
        {
            for (int i = 0; i < grassItemDatabase.Length; i++)
            {

            }
        }

        private void ClickableGrasssClickedEventHandler()
        {
            MenuManager.Instance.DisplayMenu(MenuNames.GrassListMenu, MenuOpeningType.CloseAll);
        }

        public bool IsGrassAvailable(int itemId)
        {
            for (int i = 0; i < grassGO.Length; i++)
            {
                if (grassGO[i].grass.itemId == itemId) // Check for grass as livestock needs it
                {
                    grassGO[i].RemovedGrass();
                    return true;
                }
            }
            return false;
        }

        #region Planting Mode 

        public void StartPlantingMode(int itemId)
        {
            selectedItemIdInMenu = itemId;
            isinPlantingMode = true;
            InputController.Instance.DisableDragSwipe();
        }

        public void StopPlantingMode()
        {
            if (!isinPlantingMode)
            {
                return;
            }
            selectedItemIdInMenu = -1;
            isinPlantingMode = false;
            UiGrassListMenu.Instance.StopPlantingMode();
            ChangedSometingSaveGrass();
            InputController.Instance.EnableDragSwipe();
        }

        #endregion

        public int GetGrassCountBuyId(int itemId)
        {
            if (grassGO == null)
            {
                Start();
            }
            int counter = 0;
            for (int i = 0; i < grassGO.Length; i++)
            {
                if (grassGO[i].grass.itemId == itemId)
                {
                    counter++;
                }
            }
            return counter;
        }

        public void RemoveGrass(int itemId) // will remove grass by one
        {
            for (int i = 0; i < grassGO.Length; i++)
            {
                if (grassGO[i].grass.itemId == itemId)
                {
                    grassGO[i].RemovedGrass(); // Todo: dont sequentially remove grass, it should be removed where livestock is standing
                    ChangedSometingSaveGrass();
                    return;
                }
            }
        }

        public void ChangedSometingSaveGrass()
        {
            SaveGrass();
        }

        private void SaveGrass()
        {
            for (int i = 0; i < grass.Count; i++)
            {
                grass[i] = grassGO[i].grass;
            }
            print("all grass saved");
            ES2.Save(grass, "AllGrass");
        }
    }
}

[System.Serializable]
public class Grass  // iLIST
{
    public int grassId;
    public int itemId;
    public Vector2 position;

    public Grass()
    {
    }

    public Grass(int g_id, int g_itemId, Vector2 pos)
    {
        itemId = g_itemId;
        grassId = g_id;
        position = pos;
    }
}