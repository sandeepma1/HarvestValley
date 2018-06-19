using HarvestValley.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private void Start()
    {
        PlayerController.Instance.OnPickaxeClicked += OnPickaxeClickedEventhandler;
    }

    private void OnPickaxeClickedEventhandler(int itemId)
    {
        int hit = MineralsDatabase.GetMineralsInfoById(itemId).hits;
        int output = MineralsDatabase.GetMineralsInfoById(itemId).outputId;
        print(itemId + " " + hit + " " + output);
    }
}