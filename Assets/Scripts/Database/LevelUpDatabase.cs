using UnityEngine;
using System.Text.RegularExpressions;

namespace HarvestValley.IO
{
    public class LevelUpDatabase : DatabaseBase<LevelUpDatabase>
    {
        public Level[] gameLevels;
        private string folderName = "LevelUp";

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
            gameLevels = new Level[lines.Length - 2];
            for (int i = 1; i < lines.Length - 1; i++)
            {
                chars = Regex.Split(lines[i], ",");
                gameLevels[i - 1] = new Level(IntParse(chars[0]), IntParse(chars[1]), IntParse(chars[2]),
                    IntParse(chars[3]), IntParse(chars[4]), IntParse(chars[5]), IntParse(chars[6]));
            }
        }
    }
}
[System.Serializable]
public class Level
{
    public int levelID;
    public int XPforNextLevel;
    public int fieldRewardCount;
    public int itemUnlockID;
    public int itemRewardCount;
    public int buildingUnlockID;
    public int gemsRewardCount;
    /*public int coinsRewardCount;
	public int animalsUnlockID;
	public int animalsRewardCount;
	public int productsUnlockID;
	public int productsRewardCount;
	public int productionBuildingUnlockID;
	public int productionBuildingRewardCount;
	public int decorID;*/

    public Level(int id, int xp, int field, int itemID, int buildingID, int itemCount, int gem)
    {
        levelID = id;
        XPforNextLevel = xp;
        fieldRewardCount = field;
        itemUnlockID = itemID;
        itemRewardCount = itemCount;
        buildingUnlockID = buildingID;
        gemsRewardCount = gem;
    }
}
