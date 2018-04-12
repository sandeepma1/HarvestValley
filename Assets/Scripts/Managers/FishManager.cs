using System;
using System.Collections.Generic;
using UnityEngine;
using HarvestValley.Ui;
using HarvestValley.IO;

namespace HarvestValley.Managers
{
    public class FishManager : ManagerBase<FishManager>
    {
        [SerializeField]
        private ClickableFish fishPrefab;

        private ClickableFish[] fishGO;
        private List<Fishes> fishesList = new List<Fishes>();

        private void Start()
        {
            Init();
        }

        #region Creating Fishes from save
        private void Init()
        {
            fishesList = ES2.LoadList<Fishes>("AllFishes");
            fishGO = new ClickableFish[fishesList.Count];
            for (int i = 0; i < fishesList.Count; i++)
            {
                fishGO[i] = Instantiate(fishPrefab, transform);
                fishGO[i].fish = fishesList[i];
                fishGO[i].OnFishClicked += OnFishClickedEventHandler;
            }
        }

        private void OnFishClickedEventHandler(int fishId)
        {
            print("clicked fish" + fishId);
        }
        #endregion

    }
}

public class Fishes
{
    public int itemId;
    public float fishLength;
    public string nextSpawnDateTime;

    public Fishes()
    {
    }

    public Fishes(int _itemId, float _fishLength, string _nextSpawnDateTime)
    {
        itemId = _itemId;
        fishLength = _fishLength;
        nextSpawnDateTime = _nextSpawnDateTime;
    }
}