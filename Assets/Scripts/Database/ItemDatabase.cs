using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class ItemDatabase: MonoBehaviour
{
	public static ItemDatabase m_instance = null;
	//public List<Item> items = new List<Item> ();
	public Item[] items;
	string folderName = "Items";

	void Awake ()
	{
		m_instance = this;
		Initialize ();
	}

	void Initialize ()
	{
		string[] lines = new string[100];
		string[] chars = new string[100];
		TextAsset itemCSV =	Resources.Load ("CSVs/" + folderName) as TextAsset;
		lines = Regex.Split (itemCSV.text, "\r\n");
		items = new Item[lines.Length - 2];
		for (int i = 1; i < items.Length; i++) {			
			chars = Regex.Split (lines [i], ",");
			items [i - 1] = new Item (IntParse (chars [0]), chars [1], chars [2], IntParse (chars [3]), FloatParse (chars [4]), 
				IntParse (chars [5]), (ItemType)Enum.Parse (typeof(ItemType), chars [6]), IntParse (chars [7]), (ItemSource)Enum.Parse (typeof(ItemSource), chars [8]), IntParse (chars [9]), IntParse (chars [10]),
				IntParse (chars [11]), IntParse (chars [12]), IntParse (chars [13]), IntParse (chars [14]), IntParse (chars [15]), IntParse (chars [16]), IntParse (chars [17]));
		}
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
		needID1 = itemNeedID1;
		needID2 = itemNeedID2;
		needID3 = itemNeedID3;
		needID4 = itemNeedID4;
		needAmount1 = itemNeedAmount1;
		needAmount2 = itemNeedAmount2;
		needAmount3 = itemNeedAmount3;
		needAmount4 = itemNeedAmount4;
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