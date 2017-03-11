using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerProfile : MonoBehaviour
{
	public static PlayerProfile m_instance = null;
	public TextMeshProUGUI gold, gems, stamina, level, expPoints;
	public Profile player = null;


	void Awake ()
	{
		m_instance = this;
		NewGameStart ();
		player = ES2.Load<Profile> ("playerProfile");
		IntiPlayerProfile ();
		UpdateAll ();
	}

	public bool IsGoldAvailable (int value)
	{
		if (value <= player.gold)
			return false;
		else
			return true;
	}

	public bool IsGemAvailable (int value)
	{
		if (value <= player.gems)
			return false;
		else
			return true;
	}

	public void PlayerName (string value)
	{			
		player.name = value;
		UpdateAll ();		
	}

	public void FarmName (string value)
	{			
		player.farmName = value;
		UpdateAll ();		
	}

	public void PlayerGold (int value)
	{			
		player.gold += value;
		UpdateAll ();		
	}

	public void PlayerGems (int value)
	{			
		player.gems += value;
		UpdateAll ();		
	}

	public void  PlayerLevel (int value)
	{			
		player.level += value;
		UpdateAll ();		
	}

	public void PlayerExpPoints (int value)
	{			
		player.expPoints += value;
		UpdateAll ();		
	}

	public void PlayerStamina (int value)
	{			
		player.stamina += value;
		UpdateAll ();		
	}

	void UpdateAll ()
	{
		gold.text = String.Format ("{0:### ### ### ### ###}", player.gold);
		gems.text = String.Format ("{0:### ### ### ### ###}", player.gems);
		stamina.text = player.stamina.ToString ();
		level.text = player.level.ToString ();
		expPoints.text = player.expPoints.ToString ();
		ES2.Save (player, "playerProfile");
	}

	#region Init PlayerProfile

	void NewGameStart ()
	{ 
		if (PlayerPrefs.GetInt ("playerProfile") <= 0) {			
			ES2.Delete ("playerProfile");
			player = new Profile ("PlayerName", "MyFarm", 1, 0, 1000, 10, 50, System.DateTime.UtcNow.ToString ());
			ES2.Save (player, "playerProfile");
			PlayerPrefs.SetInt ("playerProfile", 1);
		}
	}

	void IntiPlayerProfile ()
	{
		GameEventManager.playerName = player.name;
		GameEventManager.playerFarmName = player.farmName;
		GameEventManager.playerLevel = player.level;
		GameEventManager.playerExpPoints = player.expPoints;
		GameEventManager.playerGold = player.gold;
		GameEventManager.playerGems = player.gems;
		GameEventManager.playerStamina = player.stamina;
		GameEventManager.playerStaminaMaxDateTime = player.staminaMaxDateTime;
	}

	#endregion
}

//*********************************************************************************************************************

[System.Serializable]
public class Profile
{
	public string name;
	public string farmName;
	public int level;
	public int expPoints;
	public int gold;
	public int gems;
	public int stamina;
	public string staminaMaxDateTime;

	public  Profile (string p_name, string p_farmName, int p_level, int p_expPoints, int p_gold, int p_gems, int p_stamina, string p_staminaMaxDateTime)
	{
		name = p_name;
		farmName = p_farmName;
		level = p_level;
		expPoints = p_expPoints;
		gold = p_gold;
		gems = p_gems;
		stamina = p_stamina;
		staminaMaxDateTime = p_staminaMaxDateTime;
	}

	public  Profile ()
	{
		name = "Player";
		farmName = "Farm";
		level = 1;
		expPoints = 0;
		gold = 1000;
		gems = 10;
		stamina = 100;
		staminaMaxDateTime = "";
	}
}
