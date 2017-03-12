using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
	public static ItemDatabase m_instance = null;
	public List<Item> items = new List<Item> ();

	void Awake ()
	{
		m_instance = this;
		Initialize ();
	}

	void Initialize ()
	{//id, name, description, unlocksAtLevel, XP, type, gemCost, sellValueMin, sellValueMax, SellValueDefault;
		items.Add (new Item (0, Item.ItemType.Crops, "Wheat", "Grows on a feild", 1, 1, 1, 1, 5, 3));	
		items.Add (new Item (1, Item.ItemType.Crops, "Corn", "Grows on a feild", 1, 2, 1, 1, 5, 3));
		items.Add (new Item (2, Item.ItemType.Crops, "Soybean", "Grows on a feild", 1, 3, 1, 1, 5, 3));
		items.Add (new Item (3, Item.ItemType.Crops, "Sugarcane", "Grows on a feild", 1, 4, 1, 1, 5, 3));
	}
}

[System.Serializable]
public class Item
{
	public int id;
	public string name;
	public string description;
	public int unlocksAtLevel;
	public int XP;
	public ItemType type;
	public int gemCost;
	public int sellValueMin;
	public int sellValueMax;
	public int SellValueDefault;

	public Item (int itemId, ItemType itemType, string itemName, string itemDescription, int itemUnlocksAtLevel, int itemXP, int itemGemCost, int itemSellValueMin, int itemSellValueMax, int itemSellValueDefault)
	{
		id = itemId;
		name = itemName;	
		description = itemDescription;
		unlocksAtLevel = itemUnlocksAtLevel;
		XP = itemXP;
		type = itemType;
		gemCost = itemGemCost;
		sellValueMin = itemSellValueMin;
		sellValueMax = itemSellValueMax;
		SellValueDefault = itemSellValueDefault;
	}

	public enum ItemType
	{
		Crops,
		AnimalGoods,
		Suppliers,
		Products,
	}
}
