using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiBuildingMenu : Singleton<UiBuildingMenu>
{
    [SerializeField]
    private TextMeshProUGUI buildingName;
    [SerializeField]
    private Image buildingImage;
    [SerializeField]
    private Transform itemParent;
    [SerializeField]
    private Transform queueParent;

    private int selectedBuildingID = -1;
    private int selectedSourceID = -1;

    private void OnEnable()
    {
        if (FieldManager.Instance == null)
        {
            return;
        }
        selectedBuildingID = FieldManager.Instance.currentSelectedFieldID;
        selectedSourceID = FieldManager.Instance.currentlSelectedSourceID;

        if (selectedBuildingID == -1 || selectedSourceID == -1)
        {
            Debug.LogError("Selected field is -1");
            return;
        }
    }

    internal void ShowBuildingMenu()
    {

    }

}
