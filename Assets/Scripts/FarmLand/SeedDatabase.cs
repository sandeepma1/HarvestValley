using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeedDatabase : MonoBehaviour
{
	public static SeedDatabase m_instance = null;
	public Seeds[] seeds;

	void Awake ()
	{
		m_instance = this;
		Initialize ();
	}
	
	// Update is called once per frame
	void Initialize ()
	{
		seeds = new Seeds[8];
		seeds [0] = new Seeds (0, "Wheat", 1, 2, 1, 1, 1);
		seeds [1] = new Seeds (1, "Corn", 2, 2, 1, 1, 1);
		seeds [2] = new Seeds (2, "Soybean", 3, 2, 1, 2, 1);
		seeds [3] = new Seeds (3, "Sugarcane", 4, 2, 1, 3, 1);
		seeds [4] = new Seeds (4, "Carrot", 5, 2, 1, 2, 2);
		seeds [5] = new Seeds (5, "Pumpkin", 6, 2, 1, 4, 2);
		seeds [6] = new Seeds (6, "Cotton", 7, 2, 1, 5, 2);
		seeds [7] = new Seeds (7, "Lily", 8, 2, 1, 6, 2);
	}
}

public class Seeds
{
	public int id;
	public string name;
	public int minsToGrow;
	public int XP;
	public int requiredLevel;
	public int gemCost;
	public int slotID;

	public Seeds (int seedId, string seedName, int seedMinsToGrow, int seedXP, int seedRequiredLevel, int seedGemCost, int seedSlotID)
	{
		id = seedId;
		name = seedName;
		minsToGrow = seedMinsToGrow;
		XP = seedXP;
		requiredLevel = seedRequiredLevel;
		gemCost = seedGemCost;
		slotID = seedSlotID;
	}
}