using UnityEngine;
using System.Text.RegularExpressions;
using System;

namespace HarvestValley.IO
{
    public class SourceDatabase : DatabaseBase<SourceDatabase>
    {
        public SourceInfo[] sources;
        string folderName = "Source";

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            string[] lines = new string[100];
            string[] chars = new string[100];
            TextAsset itemCSV = Resources.Load("CSVs/" + folderName) as TextAsset;
            lines = Regex.Split(itemCSV.text, "\r\n");
            sources = new SourceInfo[lines.Length - 2];
            for (int i = 1; i < lines.Length - 1; i++)
            {
                chars = Regex.Split(lines[i], ",");
                sources[i - 1] = new SourceInfo(
                    IntParse(chars[0]),
                    chars[1],
                    chars[2],
                    (ItemType)Enum.Parse(typeof(ItemType), chars[3]),
                    IntParse(chars[4]),
                    IntParse(chars[5]),
                    IntParse(chars[6]),
                    IntParse(chars[7]),
                    chars[8]);
            }
        }
    }
}
[System.Serializable]
public class SourceInfo
{
    public int sourceID;
    public string name;
    public string desc;
    public ItemType type;
    public int cost;
    public int deployTime;
    public int slotsUnlocked;
    public int limit;
    public string slug;
    //shopItemType
    //public int shopitemLimit

    public SourceInfo(int b_sourceID, string b_name, string b_desc, ItemType b_type, int b_cost,
        int b_deployTime, int b_slotsUnlocked, int b_limit, string b_slug)
    {
        sourceID = b_sourceID;
        name = b_name;
        desc = b_desc;
        type = b_type;
        cost = b_cost;
        deployTime = b_deployTime;
        slotsUnlocked = b_slotsUnlocked;
        limit = b_limit;
        slug = b_slug;
    }
}