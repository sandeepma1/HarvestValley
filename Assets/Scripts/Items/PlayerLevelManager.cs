using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
	public static PlayerLevelManager m_instance = null;

	void Awake ()
	{
		m_instance = this;
	}

	public void CurrentPlayerLevel ()
	{

	}

	public void CurrentPlayerXP ()
	{

	}
}

[System.Serializable]
public class PlayerLevel
{
	public int XPforNextLevel;
	public int cropsUnlockID;
	public int animalsUnlockID;
	public int productsUnlockID;
	public int productionBuildingUnlockID;
}
