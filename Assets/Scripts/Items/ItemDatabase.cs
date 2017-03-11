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
	{
		items.Add (new Item (0, Item.ItemType.Crops, "Wheat", 1, 1, 1, 1, 5, 3));	
		items.Add (new Item (1, Item.ItemType.Crops, "Corn", 1, 1, 1, 1, 5, 3));
		items.Add (new Item (2, Item.ItemType.Crops, "Soybean", 1, 1, 1, 1, 5, 3));
		items.Add (new Item (3, Item.ItemType.Crops, "Sugarcane", 1, 1, 1, 1, 5, 3));
	}
}

[System.Serializable]
public class Item
{
	public int id;
	public string name;
	public int unlocksAtLevel;
	public int exp;
	public ItemType type;
	public int gemCost;
	public int sellValueMin;
	public int sellValueMax;
	public int SellValueDefault;

	public Item (int itemId, ItemType itemType, string itemName, int itemUnlocksAtLevel, int itemExp, int itemGemCost, int itemSellValueMin, int itemSellValueMax, int itemSellValueDefault)
	{
		id = itemId;
		name = itemName;	
		unlocksAtLevel = itemUnlocksAtLevel;
		exp = itemExp;
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
