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
            grassGO[grass.grassId].ClickableGrasssClicked += ClickableGrasssClickedEventHandler;
        }

        private void ClickableGrasssClickedEventHandler(int itemId)
        {
            if (itemId == -1)
            {
                MenuManager.Instance.DisplayMenu(MenuNames.GrassListMenu, MenuOpeningType.CloseAll);
            }
            else
            {
                // nothing at moment
            }
        }

        public bool IsAvailableGrassCount(int itemId, int amount)
        {
            int count = 0;
            for (int i = 0; i < grassGO.Length; i++)
            {
                if (grassGO[i].grass.itemId == itemId) // Check for grass as livestock needs it
                {
                    count++;
                    if (count >= amount)                //If grass is available then start removing it
                    {
                        RemoveGrass(itemId, amount);
                        return true;
                    }
                }
            }
            return false;
        }

        // Removed grass as livestock ate it
        private void RemoveGrass(int itemId, int amount)
        {
            int count = 0;
            for (int j = 0; j < grassGO.Length; j++)
            {
                if (count >= amount)
                {
                    ChangedSometingSaveGrass();
                    return;
                }
                if (grassGO[j].grass.itemId == itemId)
                {
                    grassGO[j].RemovedGrass();
                    count++;
                }
            }
            print(amount + " grass removed");
        }

        #region Planting Mode 

        public void StartPlantingMode(int itemId)
        {
            selectedItemIdInMenu = itemId;
            isinPlantingMode = true;
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
        }

        #endregion

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