using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace HarvestValley.IO
{
    public class MineralsDatabase : DatabaseBase<MineralsDatabase>
    {
        private static MineralsInfo[] minerals;
        private const string fileName = "Minerals";

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            List<string> linesList = GetAllLinesFromCSV(fileName);

            minerals = new MineralsInfo[linesList.Count];
            for (int i = 0; i < minerals.Length; i++)
            {
                string[] chars = Regex.Split(linesList[i], ",");
                minerals[i] = new MineralsInfo(
                IntParse(chars[0]),
                chars[1],
                chars[2],
                IntParse(chars[3]),
                IntParse(chars[4]),
                IntParse(chars[5]),
                IntParse(chars[6]),
                IntParse(chars[7]),
                chars[8]);
            }
        }

        public static MineralsInfo GetMineralsInfoById(int itemId)
        {
            return minerals[itemId];
        }

        public static int GetMaterialsInfolength()
        {
            return minerals.Length;
        }
    }
}
[System.Serializable]
public class MineralsInfo
{
    public int id;
    public string name;
    public string desc;
    public int outputId;
    public int min;
    public int max;
    public int hits;
    public int xp;
    public string slug;

    public MineralsInfo(int _id, string _name, string _desc, int _outputId, int _min,
        int _max, int _hits, int _xp, string _slug)
    {
        id = _id;
        name = _name;
        desc = _desc;
        outputId = _outputId;
        min = _min;
        max = _max;
        hits = _hits;
        xp = _xp;
        slug = _slug;
    }
}