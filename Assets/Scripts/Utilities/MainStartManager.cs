using System;
using UnityEngine;
using HarvestValley.Managers;
using HarvestValley.Ui;

public class MainStartManager : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyUp(KeyCode.R))
        {
            PlayerPrefs.SetInt("gameStatus", 0);
            MySceneManager.LoadScene("Start");
        }
    }

    private void OnApplicationQuit()
    {
        SaveAllGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveAllGame();
    }

    private void SaveAllGame()
    {
        PlayerProfileManager.Instance.SavePlayerProfile();
        UiInventoryMenu.Instance.SavePlayerInventory();
        GrassLandManager.Instance.SaveGrass();
        FieldManager.Instance.SaveFields();
        BuildingManager.Instance.SaveBuildings();
        LivestockManager.Instance.SaveLivestock();
    }
}
