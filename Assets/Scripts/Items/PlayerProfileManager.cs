using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerProfileManager : MonoBehaviour
{
	public static PlayerProfileManager m_instance = null;
	public TextMeshProUGUI coinsUIText, gemsUIText, staminaUIText, levelUIText, XPPointsUIText;
	public GameObject levelUpUIMenu;
	public PlayersProfile player = null;

	void Awake ()
	{
		m_instance = this;
		NewGameStart ();
		player = ES2.Load<PlayersProfile> ("playerProfile");
		IntiPlayerProfile ();
		UpdateAll ();
	}

	public bool IsGoldAvailable (int value)
	{
		if (value <= player.coins)
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

	public int CurrentPlayerLevel ()
	{
		return player.level;
	}

	public int CurrentPlayerXP ()
	{
		return player.XPPoints;
	}

	public void CheckForLevelUp ()
	{
		if (player.XPPoints >= LevelUpManager.m_instance.gameLevel [player.level].XPforNextLevel) {
			IncrementPlayerLevel ();

			FarmItemsManager.m_instance.AddNewFarmItem (LevelUpManager.m_instance.gameLevel [player.level].cropsUnlockID, LevelUpManager.m_instance.gameLevel [player.level].cropsRewardCount);
			CropMenuManager.m_instance.CheckForUnlockedSeeds ();
			levelUpUIMenu.SetActive (true);
		}
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
		player.coins += value;
		UpdateAll ();		
	}

	public void PlayerGems (int value)
	{			
		player.gems += value;
		UpdateAll ();		
	}

	public void IncrementPlayerLevel ()
	{			
		player.level++;
		UpdateAll ();		
	}

	public void PlayerXPPointsAdd (int value)
	{			
		player.XPPoints += value;
		CheckForLevelUp ();
		UpdateAll ();		
	}

	public void PlayerStamina (int value)
	{			
		player.stamina += value;
		UpdateAll ();		
	}

	void UpdateAll ()
	{
		coinsUIText.text = String.Format ("{0:### ### ### ### ###}", player.coins);
		gemsUIText.text = String.Format ("{0:### ### ### ### ###}", player.gems);
		staminaUIText.text = player.stamina.ToString ();
		levelUIText.text = player.level.ToString ();
		XPPointsUIText.text = player.XPPoints.ToString () + "/" + LevelUpManager.m_instance.gameLevel [player.level].XPforNextLevel.ToString ();
		ES2.Save (player, "playerProfile");
	}

	#region Init PlayerProfile

	void NewGameStart ()
	{ 
		if (PlayerPrefs.GetInt ("playerProfile") <= 0) {			
			ES2.Delete ("playerProfile");
			player = new PlayersProfile ("PlayerName", "MyFarm", 1, 0, 1000, 10, 50, System.DateTime.UtcNow.ToString ());
			ES2.Save (player, "playerProfile");
			PlayerPrefs.SetInt ("playerProfile", 1);
		}
	}

	void IntiPlayerProfile ()
	{
		GameEventManager.playerName = player.name;
		GameEventManager.playerFarmName = player.farmName;
		GameEventManager.playerLevel = player.level;
		GameEventManager.playerXPPoints = player.XPPoints;
		GameEventManager.playerGold = player.coins;
		GameEventManager.playerGems = player.gems;
		GameEventManager.playerStamina = player.stamina;
		GameEventManager.playerStaminaMaxDateTime = player.staminaMaxDateTime;
	}

	#endregion
}

//*********************************************************************************************************************

[System.Serializable]
public class PlayersProfile
{
	public string name;
	public string farmName;
	public int level;
	public int XPPoints;
	public int coins;
	public int gems;
	public int stamina;
	public string staminaMaxDateTime;

	public  PlayersProfile (string p_name, string p_farmName, int p_level, int p_XPPoints, int p_coins, int p_gems, int p_stamina, string p_staminaMaxDateTime)
	{
		name = p_name;
		farmName = p_farmName;
		level = p_level;
		XPPoints = p_XPPoints;
		coins = p_coins;
		gems = p_gems;
		stamina = p_stamina;
		staminaMaxDateTime = p_staminaMaxDateTime;
	}

	public  PlayersProfile ()
	{
		name = "Player";
		farmName = "Farm";
		level = 1;
		XPPoints = 0;
		coins = 1000;
		gems = 10;
		stamina = 100;
		staminaMaxDateTime = "";
	}
}
