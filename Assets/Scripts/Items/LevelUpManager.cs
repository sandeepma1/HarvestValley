using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class LevelUpManager : MonoBehaviour
{
	public static LevelUpManager m_instance = null;
	//public List<Level> gameLevel = new List<Level> ();
	public Level[] gameLevels;

	void Awake ()
	{
		m_instance = this;
		Initialize ();
	}

	void Initialize ()
	{
		string[] lines = new string[100];
		string[] chars = new string[100];
		TextAsset itemCSV =	Resources.Load ("CSVs/LevelUp") as TextAsset;
		lines = Regex.Split (itemCSV.text, "\r\n");
		gameLevels = new Level[lines.Length - 2];
		for (int i = 1; i < lines.Length - 1; i++) {			
			chars = Regex.Split (lines [i], ",");
			gameLevels [i - 1] = new Level (IntParse (chars [0]), IntParse (chars [1]), IntParse (chars [2]), 
				IntParse (chars [3]), IntParse (chars [4]), IntParse (chars [5]), IntParse (chars [6]));
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
public class Level
{
	public int levelID;
	public int XPforNextLevel;
	public int fieldRewardCount;
	public int cropsUnlockID;
	public int buildingUnlockID;
	public int cropsRewardCount;
	public int gemsRewardCount;
	/*public int coinsRewardCount;
	public int animalsUnlockID;
	public int animalsRewardCount;
	public int productsUnlockID;
	public int productsRewardCount;
	public int productionBuildingUnlockID;
	public int productionBuildingRewardCount;
	public int decorID;*/

	public Level (int id, int xp, int field, int cropID, int buildingID, int cropCount, int gem)
	{
		levelID = id;
		XPforNextLevel = xp;
		fieldRewardCount = field;
		cropsUnlockID = cropID;
		buildingUnlockID = buildingID;
		cropsRewardCount = cropCount;
		gemsRewardCount = gem;
	}
}
