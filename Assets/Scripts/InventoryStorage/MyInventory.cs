using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInventory : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

public class Items
{
	public int id;
	public string name;
	public int minsToGrow;
	public int exp;
	public int requiredLevel;
	public int gemCost;

	public Items (int seedId, string seedName, int seedMinsToGrow, int seedExp, int seedRequiredLevel, int seedGemCost)
	{
		id = seedId;
		name = seedName;
		minsToGrow = seedMinsToGrow;
		exp = seedExp;
		requiredLevel = seedRequiredLevel;
		gemCost = seedGemCost;
	}
}