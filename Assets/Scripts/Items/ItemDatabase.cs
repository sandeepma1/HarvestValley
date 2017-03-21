using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class ItemDatabase : MonoBehaviour
{
	public static ItemDatabase m_instance = null;
	//public List<Item> items = new List<Item> ();
	public Item[] items;

	void Awake ()
	{
		m_instance = this;
		Initialize ();
	}

	void Initialize ()
	{
		string[] lines = new string[100];
		string[] chars = new string[100];
		TextAsset itemCSV =	Resources.Load ("CSVs/Items") as TextAsset;
		lines = Regex.Split (itemCSV.text, "\r\n");
		items = new Item[lines.Length - 2];
		for (int i = 1; i < lines.Length - 1; i++) {			
			chars = Regex.Split (lines [i], ",");
			items [i - 1] = new Item (IntParse (chars [0]), chars [1], chars [2], IntParse (chars [3]), FloatParse (chars [4]), 
				IntParse (chars [5]), (ItemType)Enum.Parse (typeof(ItemType), chars [6]), IntParse (chars [7]), (ItemSource)Enum.Parse (typeof(ItemSource), chars [8]), IntParse (chars [9]), IntParse (chars [10]),
				IntParse (chars [11]), IntParse (chars [12]), IntParse (chars [13]), IntParse (chars [14]), IntParse (chars [15]), IntParse (chars [16]), IntParse (chars [17]));
		}

		/*items = new Item[12];
		items [0] = new Item (0, "Wheat", "Descriptions", 1, 2, 2, ItemType.Crops, 2, ItemSource.Field, 10, 0, 1, -1, 0, -1, 0, -1, 0);
		items [1] = new Item (1, "Corn", "Descriptions", 1, 5, 3, ItemType.Crops, 3, ItemSource.Field, 15, 1, 1, -1, 0, -1, 0, -1, 0);
		items [2] = new Item (2, "Soybean", "Descriptions", 1, 7, 4, ItemType.Crops, 4, ItemSource.Field, 20, 2, 1, -1, 0, -1, 0, -1, 0);
		items [3] = new Item (3, "Sugarcane", "Descriptions", 2, 7, 5, ItemType.Crops, 5, ItemSource.Field, 25, 3, 1, -1, 0, -1, 0, -1, 0);

		items [4] = new Item (4, "Carrot", "Descriptions", 1, 7, 4, ItemType.Crops, 4, ItemSource.Field, 10, 2, 1, -1, 0, -1, 0, -1, 0);
		items [5] = new Item (5, "Wheat", "Descriptions", 2, 7, 4, ItemType.Crops, 4, ItemSource.Field, 10, 2, 1, -1, 0, -1, 0, -1, 0);
		items [6] = new Item (6, "Wheat", "Descriptions", 2, 7, 4, ItemType.Crops, 4, ItemSource.Field, 10, 2, 1, -1, 0, -1, 0, -1, 0);
		items [7] = new Item (7, "Wheat", "Descriptions", 2, 7, 4, ItemType.Crops, 4, ItemSource.Field, 10, 2, 1, -1, 0, -1, 0, -1, 0);

		items [8] = new Item (8, "Wheat", "Descriptions", 2, 7, 4, ItemType.Crops, 4, ItemSource.Field, 10, 2, 1, -1, 0, -1, 0, -1, 0);
		items [9] = new Item (9, "Wheat", "Descriptions", 2, 7, 4, ItemType.Crops, 4, ItemSource.Field, 10, 2, 1, -1, 0, -1, 0, -1, 0);
		items [10] = new Item (10, "Wheat", "Descriptions", 2, 7, 4, ItemType.Crops, 4, ItemSource.Field, 10, 2, 1, -1, 0, -1, 0, -1, 0);
		items [11] = new Item (11, "Wheat", "Descriptions", 2, 7, 4, ItemType.Crops, 4, ItemSource.Field, 10, 2, 1, -1, 0, -1, 0, -1, 0);*/
	}

	int IntParse (string text)
	{
		int num;
		if (int.TryParse (text, out num)) {
			return num;
		} else
			return 0;
	}

	float FloatParse (string text)
	{
		float result = 0.01f;
		float.TryParse (text, out result);
		return result;

	}
	/*ItemType EnumParseItemType (string text)
	{
		ItemType item;
		if (Enum.Parse (text, out item)) {
			return item;
		} else
			return 0;
	}*/
}

[System.Serializable]
public class Item
{
	//ID, Name, Level, Max. price, Time, XP, NeedsID1, NeedsAmount1, NeedsID2, NeedsAmount2, NeedsID3, NeedsAmount3, NeedsID4, NeedsAmount4, Source, PerBoatCrateMIN, PerBoatCrateMAX

	public int id;
	public string name;
	public string description;
	public int unlocksAtLevel;
	public float timeRequiredInMins;
	public int XP;
	public ItemType type;
	public int gemCost;
	public ItemSource source;
	public int sellValueMax;
	public int needID1;
	public int needAmount1;
	public int needID2;
	public int needAmount2;
	public int needID3;
	public int needAmount3;
	public int needID4;
	public int needAmount4;

	public Item (int itemId, string itemName, string itemDescription, int itemUnlocksAtLevel, 
	             float itemTimeRequiredInMins, int itemXP, ItemType itemType, int itemGemCost, ItemSource itemSource, 
	             int itemSellValueMax, int itemNeedID1, int itemNeedAmount1, int itemNeedID2, int itemNeedAmount2,
	             int itemNeedID3, int itemNeedAmount3, int itemNeedID4, int itemNeedAmount4)
	{
		id = itemId;
		name = itemName;	
		description = itemDescription;
		unlocksAtLevel = itemUnlocksAtLevel;
		timeRequiredInMins = itemTimeRequiredInMins;
		XP = itemXP;
		type = itemType;
		gemCost = itemGemCost;
		sellValueMax = itemSellValueMax;
		source = itemSource;
	}
}

public enum ItemType
{
	Crops,
	AnimalGoods,
	Suppliers,
	Products,
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
	BbqGrill,
	Pig,
	PieOven,
	AppleTree,
	Sheep,
	Loom,
	RaspberryBush,
	SewingMachine,
	CakeOven,
	CherryTree,
	Mine,
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
	HoneyExtractor
}