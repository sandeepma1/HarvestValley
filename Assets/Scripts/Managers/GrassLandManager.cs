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
        [HideInInspector]
        public bool isInPlantingMode = false;

        private ClickableGrass[,] grassGO;
        public List<Grass> grass = new List<Grass>();
        public int selectedItemIdInMenu;

        private Dictionary<int, int> grassItemDatabase;

        private void Start()
        {
            grass = ES2.LoadList<Grass>("AllGrass");
            Vector2 grassLandDimension = grass[grass.Count - 1].position;
            int xBound = (int)grassLandDimension.x + 1;
            int yBound = (int)-grassLandDimension.y + 1;
            grassGO = new ClickableGrass[xBound, xBound]; // the - thing is due to the level goes down by Y

            int counter = 0;
            for (int i = 0; i < xBound; i++)
            {
                for (int j = 0; j < yBound; j++)
                {
                    grassGO[i, j] = InitGrassPatches(grass[counter]);
                    counter++;
                }
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
            isInPlantingMode = true;
            InputController.Instance.DisableDragSwipe();
        }

        public void StopPlantingMode()
        {
            if (!isInPlantingMode)
            {
                return;
            }
            selectedItemIdInMenu = -1;
            isInPlantingMode = false;
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


        //DO NOT DELETE
        //Good to find the nearest grass and remove it
        public Vector2 GetNearestGrass(int itemId, Vector3 currentPos)
        {
            int posX = (int)currentPos.x;
            int posY = (int)currentPos.y;

            Vector2 grassLandDimension = grass[grass.Count - 1].position;
            int xBound = (int)grassLandDimension.x + 1;
            int yBound = (int)-grassLandDimension.y + 1;
            Vector2 tMin = Vector2.one;
            float minDist = Mathf.Infinity;

            List<Vector2> nearest = new List<Vector2>();
            for (int i = 0; i < xBound; i++)
            {
                for (int j = 0; j < yBound; j++)
                {
                    if (grassGO[i, j].grass.itemId == itemId)
                    {
                        Vector2 pos = new Vector2(i, j);
                        float dist = Vector3.Distance(pos, currentPos);
                        if (dist < minDist)
                        {
                            tMin = pos;
                            minDist = dist;
                        }
                    }
                }
            }
            Vector2 newPos = new Vector2(tMin.x, -tMin.y);
            grassGO[(int)tMin.x, (int)tMin.y].RemovedGrass();
            return newPos;
        }

        public void RemoveGrass(int itemId) // will remove grass by one
        {
            Vector2 grassLandDimension = grass[grass.Count - 1].position;
            int xBound = (int)grassLandDimension.x + 1;
            int yBound = (int)-grassLandDimension.y + 1;

            for (int i = 0; i < xBound; i++)
            {
                for (int j = 0; j < yBound; j++)
                {
                    if (grassGO[i, j].grass.itemId == itemId)
                    {
                        grassItemDatabase[itemId]--;
                        grassGO[i, j].RemovedGrass(); // Todo: dont sequentially remove grass, it should be removed where livestock is standing                    
                        return;
                    }
                }
            }
        }

        private void RemoveGrassRandomly()
        {

        }

        public void SaveGrass()
        {
            Vector2 grassLandDimension = grass[grass.Count - 1].position;
            int xBound = (int)grassLandDimension.x + 1;
            int yBound = (int)-grassLandDimension.y + 1;
            int counter = 0;
            for (int i = 0; i < xBound; i++)
            {
                for (int j = 0; j < yBound; j++)
                {
                    grass[counter] = grassGO[i, j].grass;
                    counter++;
                }
            }

            ES2.Save(grass, "AllGrass");
        }

        private void Resize2DArray<T>(ref T[,] original, int newCoNum, int newRoNum)
        {
            var newArray = new T[newCoNum, newRoNum];
            int columnCount = original.GetLength(1);
            int columnCount2 = newRoNum;
            int columns = original.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
            original = newArray;
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