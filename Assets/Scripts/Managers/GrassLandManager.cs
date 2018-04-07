using System.Collections.Generic;
using UnityEngine;
using HarvestValley.Ui;
using System;
using System.Collections;

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

        private Dictionary<int, int> grassItemDatabase;

        private void Start()
        {
            grass = ES2.LoadList<Grass>("AllGrass");
            grassGO = new ClickableGrass[grass.Count];
            for (int i = 0; i < grass.Count; i++)
            {
                InitGrassPatches(grass[i]);
            }
            StartCoroutine("WaitForEndOfFrame");
        }

        public IEnumerator WaitForEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            GetAllGrassItemIds();
        }

        private void InitGrassPatches(Grass grass)
        {
            grassGO[grass.grassId] = Instantiate(clickableGrassPrefab, transform);
            grassGO[grass.grassId].grass = grass;
            grassGO[grass.grassId].transform.localPosition = grass.position;
            grassGO[grass.grassId].gameObject.name = "Grass" + grass.grassId;
            grassGO[grass.grassId].OpenUiGrassMenu += OpenUiGrassMenuEventHandler;
            grassGO[grass.grassId].ClickableGrassAddedItem += ClickableGrassAddedItemEventHandler;
        }

        private void GetAllGrassItemIds()
        {
            int[] grassItemIds = UiGrassListMenu.Instance.GetAllGrassItemIds();
            grassItemDatabase = new Dictionary<int, int>();// new InventoryItems[grassItemIds.Length];          
            for (int i = 0; i < grassItemIds.Length; i++)
            {
                grassItemDatabase.Add(grassItemIds[i], 0);
            }
            if (grass.Count == 0)
            {
                Start();
            }
            for (int i = 0; i < grass.Count; i++)
            {
                if (grassItemDatabase.ContainsKey(grass[i].itemId))
                {
                    grassItemDatabase[grass[i].itemId]++;
                }
            }
        }

        private void ClickableGrassAddedItemEventHandler(int itemId)
        {
            grassItemDatabase[itemId]++;
        }

        private void OpenUiGrassMenuEventHandler()
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
            InputController.Instance.EnableDragSwipe();
        }

        #endregion

        public int GetGrassCountBuyId(int itemId)
        {
            if (grassItemDatabase == null)
            {
                return 0;
            }

            if (grassItemDatabase.ContainsKey(itemId))
            {
                return grassItemDatabase[itemId];
            }

            return 0;
        }

        public void RemoveGrass(int itemId) // will remove grass by one
        {
            for (int i = 0; i < grassGO.Length; i++)
            {
                if (grassGO[i].grass.itemId == itemId)
                {
                    grassItemDatabase[itemId]--;
                    grassGO[i].RemovedGrass(); // Todo: dont sequentially remove grass, it should be removed where livestock is standing                    
                    return;
                }
            }
        }

        public void SaveGrass()
        {
            for (int i = 0; i < grass.Count; i++)
            {
                grass[i] = grassGO[i].grass;
            }
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