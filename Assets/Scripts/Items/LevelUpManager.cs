using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
	public static LevelUpManager m_instance = null;
	public List<Level> gameLevel = new List<Level> ();

	void Awake ()
	{
		m_instance = this;
		Initialize ();
	}

	void Initialize ()
	{
		gameLevel.Add (new Level (0, 3, 1, 4, 10));
		gameLevel.Add (new Level (20, 0, 2, 4, 10));
		gameLevel.Add (new Level (50, 3, 3, 4, 10));
		gameLevel.Add (new Level (150, 0, 4, 4, 10));
	}
}

[System.Serializable]
public class Level
{
	public int XPforNextLevel;
	public int fieldRewardCount;
	public int cropsUnlockID;
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

	public Level (int xp, int field, int cropID, int cropCount, int gem)
	{
		XPforNextLevel = xp;
		fieldRewardCount = field;
		cropsUnlockID = cropID;
		cropsRewardCount = cropCount;
		gemsRewardCount = gem;
	}
}
