using System.Collections.Generic;
using UnityEngine;
using HarvestValley.Ui;

namespace HarvestValley.Managers
{
    public class GrassLandManager : ManagerBase<GrassLandManager>
    {
        [SerializeField]
        private ClickableGrass clickableGrassPrefab;
        [SerializeField]
        private int x = 12, y = 12;
        public bool isPlantingMode = false;

        private ClickableGrass[] grassGO;
        private List<Grass> grass = new List<Grass>();
        private bool isInPlantingMode;

        private void Start()
        {
            grass = ES2.LoadList<Grass>("AllGrass");
            grassGO = new ClickableGrass[grass.Count];

            for (int i = 0; i < grass.Count; i++)
            {
                InitFields(grass[i]);
            }
        }

        private void InitFields(Grass grass)
        {
            grassGO[grass.grassId] = Instantiate(clickableGrassPrefab, transform);
            grassGO[grass.grassId].grass = grass;
            grassGO[grass.grassId].transform.localPosition = grass.position;
            grassGO[grass.grassId].gameObject.name = "Grass" + grass.grassId;
        }

        #region Planting Mode

        public void StartPlantingMode()
        {
            isInPlantingMode = true;
        }

        public void StopPlantingMode()
        {
            if (!isInPlantingMode)
            {
                return;
            }

            isInPlantingMode = false;
            UiGrassListMenu.Instance.StopPlantingMode();
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