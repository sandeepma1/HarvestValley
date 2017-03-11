using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFarmLand  // iLIST
{
	public int id;
	public Vector2 pos;
	public int level;
	public int seedID;
	public int state;
	public string dateTime;

	public SaveFarmLand ()
	{		
		id = -1;
		pos = new Vector2 (0, 0);
		level = -1;
		seedID = -1;
		state = -1;
		dateTime = "";
	}

	public SaveFarmLand (int f_id, Vector2 f_pos, int f_level, int f_seedID, int f_state, string f_dateTime)
	{		
		id = f_id;
		pos = f_pos;
		level = f_level;
		seedID = f_seedID;
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