using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class BuildingDatabase : MonoBehaviour
{
    public static BuildingDatabase Instance = null;
    public BuildingInfo[] buildingInfo;
    string folderName = "Source";

    void Awake()
    {
        Instance = this;
        Initialize();
    }

    void Initialize()
    {
        string[] lines = new string[100];
        string[] chars = new string[100];
        TextAsset itemCSV = Resources.Load("CSVs/" + folderName) as TextAsset;
        lines = Regex.Split(itemCSV.text, "\r\n");
        buildingInfo = new BuildingInfo[lines.Length - 2];
        for (int i = 1; i < lines.Length - 1; i++)
        {
            chars = Regex.Split(lines[i], ",");
            buildingInfo[i - 1] = new BuildingInfo(IntParse(chars[0]), (ItemSource)Enum.Parse(typeof(ItemSource), chars[1]), chars[2], (ItemType)Enum.Parse(typeof(ItemType), chars[3]),
                IntParse(chars[4]), IntParse(chars[5]), IntParse(chars[6]), IntParse(chars[7]));
        }
    }
    //public BuildingInfo (int b_id, string b_name, string b_desc, ItemType b_type, int b_cost, int b_deployTime, int b_slotsUnlocked, int b_limit)

    int IntParse(string text)
    {
        int num;
        if (int.TryParse(text, out num))
        {
            return num;
        } else
            return 0;
    }

    float FloatParse(string text)
    {
        float result = 0.01f;
        float.TryParse(text, out result);
        return result;
    }
}

[System.Serializable]
public class BuildingInfo
{
    public int id;
    public ItemSource name;
    public string desc;
    public ItemType type;
    public int cost;
    public int deployTime;
    public int slotsUnlocked;
    public int limit;
    //shopItemType
    //public int shopitemLimit

    public BuildingInfo(int b_id, ItemSource b_name, string b_desc, ItemType b_type, int b_cost, int b_deployTime, int b_slotsUnlocked, int b_limit)
    {
        id = b_id;
        name = b_name;
        desc = b_desc;
        type = b_type;
        cost = b_cost;
        deployTime = b_deployTime;
        slotsUnlocked = b_slotsUnlocked;
        limit = b_limit;
    }
}
