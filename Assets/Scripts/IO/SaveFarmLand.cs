using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFarmLand  // iLIST
{
	public sbyte tileIndex;
	public sbyte level;
	public sbyte seedIndex;
	public sbyte state;
	public string dateTime;

	public SaveFarmLand ()
	{		
		tileIndex = -1;
		level = -1;
		seedIndex = -1;
		state = -1;
		dateTime = "";
	}

	public SaveFarmLand (sbyte f_tileIndex, sbyte f_level, sbyte f_seedIndex, sbyte f_state, string f_dateTime)
	{		
		tileIndex = f_tileIndex;
		level = f_level;
		seedIndex = f_seedIndex;
		state = f_state;
		dateTime = f_dateTime;
	}
}

public enum FARM_LAND_STATE
{
	NONE,
	GROWING,
	WAITING_FOR_HARVEST}

;