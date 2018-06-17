using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace HarvestValley.IO
{
    public class ItemDatabase : DatabaseBase<ItemDatabase>
    {
        private static Item[] items;
        private const string fileName = "Items";

        public static int AllItemCount
        {
            get
            {
                return items.Length;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            List<string> linesList = GetAllLinesFromCSV(fileName);

            items = new Item[linesList.Count];
            for (int i = 0; i < items.Length; i++)
            {
                string[] chars = Regex.Split(linesList[i], ",");
                items[i] = new Item(
                    IntParse(chars[0]),
                    chars[1],
                    chars[2],
                    IntParse(chars[3]),
                    IntParse(chars[4]),
                    IntParse(chars[5]),
                    (ItemType)Enum.Parse(typeof(ItemType), chars[6]),
                    IntParse(chars[7]),
                    IntParse(chars[8]),
                    IntParse(chars[9]),
                    IntParse(chars[10]),
                    IntParse(chars[11]),
                    IntParse(chars[12]),
                    IntParse(chars[13]),
                    IntParse(chars[14]),
                    IntParse(chars[15]),
                    new int[] { IntParse(chars[16]), IntParse(chars[18]), IntParse(chars[20]), IntParse(chars[22]) },
                    new int[] { IntParse(chars[17]), IntParse(chars[19]), IntParse(chars[21]), IntParse(chars[23]) },
                    chars[24]);
            }
        }

        public static Item GetItemById(int itemId)
        {
            return items[itemId];
        }

        public static string GetItemSlugById(int itemId)
        {
            return items[itemId].slug;
        }

        public static int GetItemslength()
        {
            return items.Length;
        }



        public static Item[] GetAllItemsBySourceId(int sourceId)
        {

            List<Item> tempItems = new List<Item>();
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].sourceID == sourceId)
                {
                    tempItems.Add(items[i]);
                }
            }
            return tempItems.ToArray();
        }
    }
}

[System.Serializable]
public class Item
{
    //ID, Name, Level, Max. price, Time, XP, NeedsID1, NeedsAmount1, NeedsID2, NeedsAmount2, NeedsID3, NeedsAmount3, NeedsID4, NeedsAmount4, Source, PerBoatCrateMIN, PerBoatCrateMAX

    public int itemID;                  //0
    public string name;                 //1
    public string description;
    public int unlocksAtLevel;
    public int timeRequiredInSeconds;
    public int XPperYield;
    public ItemType type;
    public int coinCost;
    public int gemCost;
    public int sourceID;

    public int noOfWatering;            //10
    public int baseYieldMin;
    public int baseYieldMax;
    public int sellValueMin;
    public int sellValueMax;
    public int noOfFertilizer;

    //16
    public int[] needID;
    public int[] needAmount;

    public string slug;

    public Item(int itemId, string itemName, string itemDescription, int itemUnlocksAtLevel,
                 int itemTimeRequiredInSeconds, int itemXP, ItemType itemType, int itemCoinCost, int itemGemCost, int itemSourceID,
                 int itemNoOfWatering, int itemBaseYieldMin, int itemBaseYieldMax, int itemSellValueMin, int itemSellValueMax,
                 int itemNoOfFertilizer, int[] itemNeedID, int[] itemNeedAmount, string itemSlug)
    {
        itemID = itemId;
        name = itemName;
        description = itemDescription;
        unlocksAtLevel = itemUnlocksAtLevel;
        timeRequiredInSeconds = itemTimeRequiredInSeconds;
        XPperYield = itemXP;
        type = itemType;
        coinCost = itemCoinCost;
        gemCost = itemGemCost;
        sourceID = itemSourceID;

        noOfWatering = itemNoOfWatering;
        baseYieldMin = itemBaseYieldMin;
        baseYieldMax = itemBaseYieldMax;
        sellValueMin = itemSellValueMin;
        sellValueMax = itemSellValueMax;
        noOfFertilizer = itemNoOfFertilizer;

        sellValueMax = itemSellValueMax;
        needID = itemNeedID;
        needAmount = itemNeedAmount;

        slug = itemSlug;
    }
}

public enum ItemType
{
    Crops,
    AnimalGoods,
    Suppliers,
    Products,
    Grass,
    Fish,
    Minerals
}

public enum ItemSource
{
    Field,
    Chicken,
    Bakery,
    FeedMill,
    Cow,
    Dairy,
    SugarMill,
    PopcornPot,
    BBQGrill,
    Pig,
    PieOven,
    AppleTree,
    Sheep,
    Loom,
    RaspberryBush,
    SewingMachine,
    CakeOven,
    CherryTree,
    Mines,
    Smelter,
    BlackberryBush,
    JuicePress,
    Fish,
    LureWorkbench,
    IceCreamMaker,
    NetMaker,
    Goat,
    JamMaker,
    CacaoTree,
    Jeweler,
    BeehiveTree,
    HoneyExtractor,
    GrassTile
}