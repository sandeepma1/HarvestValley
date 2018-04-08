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
                grassGO[i] = InitGrassPatches(grass[i]);
            }
            StartCoroutine("WaitForEndOfFrame");
        }

        public IEnumerator WaitForEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            GetAllGrassItemIds();
        }

        private ClickableGrass InitGrassPatches(Grass grass)
        {
            ClickableGrass clickableGrass = Instantiate(clickableGrassPrefab, transform);
            clickableGrass.grass = grass;
            clickableGrass.transform.localPosition = grass.position;
            clickableGrass.OpenUiGrassMenu += OpenUiGrassMenuEventHandler;
            clickableGrass.ClickableGrassAddedItem += ClickableGrassAddedItemEventHandler;
            return clickableGrass;
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
            //int tempId = grassGO.RandomItem().grass.itemId;
            //print(tempId);
            //if (tempId == -1)
            //{
            //    //RemoveGrass(itemId);
            //}

            //if (grassGO[tempId].grass.itemId == itemId)
            //{
            //    print("got it removed");
            //    grassItemDatabase[itemId]--;
            //    grassGO[tempId].RemovedGrass();
            //    return;
            //}
            //else
            //{
            //    print("going again");
            //    RemoveGrass(itemId);
            //}

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

        private void RemoveGrassRandomly()
        {

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

public static class ArrayExtensions
{
    // This is an extension method. RandomItem() will now exist on all arrays.
    public static T RandomItem<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }
}

[System.Serializable]
public class Grass  // iLIST
{
    public int itemId;
    public Vector2 position;

    public Grass()
    {
    }

    public Grass(int g_itemId, Vector2 pos)
    {
        itemId = g_itemId;
        position = pos;
    }
}