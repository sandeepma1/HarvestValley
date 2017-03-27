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
	public PlayersProfile playerProfile = null;

	bool isUpdated = false;
	float updateTimer = 1f;

	void Awake ()
	{
		m_instance = this;
		NewGameStart ();
		playerProfile = ES2.Load<PlayersProfile> ("playerProfile");
		InitPlayerProfile ();
		UpdateAll ();
	}

	public bool IsGoldAvailable (int value)
	{
		if (value <= playerProfile.coins)
			return false;
		else
			return true;
	}

	public bool IsGemAvailable (int value)
	{
		if (value <= playerProfile.gems)
			return false;
		else
			return true;
	}

	public int CurrentPlayerLevel ()
	{
		return playerProfile.level;
	}

	public int CurrentPlayerXP ()
	{
		return playerProfile.XPPoints;
	}

	public void CheckForLevelUp ()
	{
		if (playerProfile.XPPoints >= LevelUpManager.m_instance.gameLevels [playerProfile.level].XPforNextLevel) {
			IncrementPlayerLevel ();
			if (LevelUpManager.m_instance.gameLevels [playerProfile.level].cropsUnlockID >= 0) {				
				PlayerInventoryManager.m_instance.AddNewFarmItem (LevelUpManager.m_instance.gameLevels [playerProfile.level].cropsUnlockID,
					LevelUpManager.m_instance.gameLevels [playerProfile.level].cropsRewardCount);
				CropMenuManager.m_instance.CheckForUnlockedSeeds ();
				PlayerGems (LevelUpManager.m_instance.gameLevels [playerProfile.level].gemsRewardCount);
			}

			PlayerXPPointsAdd (-CurrentPlayerXP ()); 
			levelUpUIMenu.SetActive (true);
			//MASTER_SaveEverything.m_instance.SaveGameDays (1);
		}
		UpdateAll ();
	}

	public void PlayerName (string value)
	{			
		playerProfile.name = value;
		UpdateAll ();		
	}

	public void FarmName (string value)
	{			
		playerProfile.farmName = value;
		UpdateAll ();		
	}

	public void PlayerGold (int value)
	{			
		playerProfile.coins += value;
		UpdateAll ();		
	}

	public void PlayerGems (int value)
	{			
		playerProfile.gems += value;
		UpdateAll ();		
	}

	public void IncrementPlayerLevel ()
	{			
		playerProfile.level++;
		UpdateAll ();		
	}

	public void PlayerXPPointsAdd (int value)
	{			
		playerProfile.XPPoints += value;
		CheckForLevelUp ();
		UpdateAll ();		
	}

	public void PlayerStamina (int value)
	{			
		playerProfile.stamina += value;
		UpdateAll ();		
	}

	void UpdateAll ()
	{
		coinsUIText.text = String.Format ("{0:### ### ### ### ###}", playerProfile.coins);
		gemsUIText.text = String.Format ("{0:### ### ### ### ###}", playerProfile.gems);
		staminaUIText.text = playerProfile.stamina.ToString ();
		levelUIText.text = playerProfile.level.ToString ();
		XPPointsUIText.text = playerProfile.XPPoints.ToString () + "/" + LevelUpManager.m_instance.gameLevels [playerProfile.level].XPforNextLevel.ToString ();
		StopCoroutine ("SavePlayerProfile");
		StartCoroutine ("SavePlayerProfile");
	}

	IEnumerator SavePlayerProfile ()
	{
		yield return new WaitForSeconds (1f);
		ES2.Save (playerProfile, "playerProfile");
		print ("saved player profile");
	}

	#region Init PlayerProfile

	void NewGameStart ()
	{ 
		if (PlayerPrefs.GetInt ("playerProfile") <= 0) {			
			ES2.Delete ("playerProfile");
			playerProfile = new PlayersProfile ("PlayerName", "MyFarm", 1, 0, 1000, 10, 50, System.DateTime.UtcNow.ToString ());
			ES2.Save (playerProfile, "playerProfile");
			PlayerPrefs.SetInt ("playerProfile", 1);
		}
	}

	void InitPlayerProfile ()
	{
		GameEventManager.playerName = playerProfile.name;
		GameEventManager.playerFarmName = playerProfile.farmName;
		GameEventManager.playerLevel = playerProfile.level;
		GameEventManager.playerXPPoints = playerProfile.XPPoints;
		GameEventManager.playerGold = playerProfile.coins;
		GameEventManager.playerGems = playerProfile.gems;
		GameEventManager.playerStamina = playerProfile.stamina;
		GameEventManager.playerStaminaMaxDateTime = playerProfile.staminaMaxDateTime;
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
