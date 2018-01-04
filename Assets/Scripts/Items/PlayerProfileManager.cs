using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager Instance = null;
    [SerializeField]
    public TextMeshProUGUI coinsUIText, gemsUIText, staminaUIText, levelUIText, XPPointsUIText;
    private PlayersProfile playerProfile = null;
    private bool isLevelUpReady;

    void Awake()
    {
        Instance = this;
        NewGameStart();
        playerProfile = ES2.Load<PlayersProfile>("playerProfile");
        InitPlayerProfile();
        UpdateAll();
    }

    void LateUpdate()
    {
        if (isLevelUpReady && !Input.anyKey)
        {
            isLevelUpReady = false;
            MenuManager.Instance.LevelUpMenuSetActive(true);
        }
    }

    public bool IsGoldAvailable(int value)
    {
        if (value <= playerProfile.coins)
            return false;
        else
            return true;
    }

    public bool IsGemAvailable(int value)
    {
        if (value <= playerProfile.gems)
            return false;
        else
            return true;
    }

    public int CurrentPlayerLevel()
    {
        return playerProfile.level;
    }

    public int CurrentPlayerXP()
    {
        return playerProfile.XPPoints;
    }

    public void CheckForLevelUp()
    {
        if (playerProfile.XPPoints >= LevelUpDatabase.Instance.gameLevels[playerProfile.level].XPforNextLevel)
        {
            IncrementPlayerLevel();
            if (LevelUpDatabase.Instance.gameLevels[playerProfile.level].itemUnlockID >= 0)
            {
                PlayerInventoryManager.Instance.AddNewFarmItem(LevelUpDatabase.Instance.gameLevels[playerProfile.level].itemUnlockID,
                    LevelUpDatabase.Instance.gameLevels[playerProfile.level].itemRewardCount);
                MasterMenuManager.Instance.CheckForUnlockedItems();
                UIMasterMenuManager.Instance.CheckForUnlockedItems();
                PlayerGems(LevelUpDatabase.Instance.gameLevels[playerProfile.level].gemsRewardCount);
            }
            PlayerXPPointsAdd(-CurrentPlayerXP());
            isLevelUpReady = true;
        }
        UpdateAll();
    }

    public void PlayerName(string value)
    {
        playerProfile.name = value;
        UpdateAll();
    }

    public void FarmName(string value)
    {
        playerProfile.farmName = value;
        UpdateAll();
    }

    public void PlayerGold(int value)
    {
        playerProfile.coins += value;
        UpdateAll();
    }

    public void PlayerGems(int value)
    {
        playerProfile.gems += value;
        UpdateAll();
    }

    public void IncrementPlayerLevel()
    {
        playerProfile.level++;
        UpdateAll();
    }

    public void PlayerXPPointsAdd(int value)
    {
        playerProfile.XPPoints += value;
        CheckForLevelUp();
        UpdateAll();
    }

    public void PlayerStamina(int value)
    {
        playerProfile.stamina += value;
        UpdateAll();
    }

    void UpdateAll()
    {
        coinsUIText.text = String.Format("{0:###,###,###,###,###}", playerProfile.coins);
        gemsUIText.text = String.Format("{0:###,###,###,###,###}", playerProfile.gems);
        staminaUIText.text = playerProfile.stamina.ToString();
        levelUIText.text = playerProfile.level.ToString();
        XPPointsUIText.text = playerProfile.XPPoints.ToString() + "/" + LevelUpDatabase.Instance.gameLevels[playerProfile.level].XPforNextLevel.ToString();
        StopCoroutine("SavePlayerProfile");
        StartCoroutine("SavePlayerProfile");
    }

    IEnumerator SavePlayerProfile()
    {
        yield return new WaitForSeconds(1f);
        ES2.Save(playerProfile, "playerProfile");
    }

    #region Init PlayerProfile

    void NewGameStart()
    {
        if (PlayerPrefs.GetInt("playerProfile") <= 0)
        {
            ES2.Delete("playerProfile");
            playerProfile = new PlayersProfile("PlayerName", "MyFarm", 1, 0, 1000, 10, 50, System.DateTime.UtcNow.ToString());
            ES2.Save(playerProfile, "playerProfile");
            PlayerPrefs.SetInt("playerProfile", 1);
        }
    }

    void InitPlayerProfile()
    {
        GEM.playerName = playerProfile.name;
        GEM.playerFarmName = playerProfile.farmName;
        GEM.playerLevel = playerProfile.level;
        GEM.playerXPPoints = playerProfile.XPPoints;
        GEM.playerGold = playerProfile.coins;
        GEM.playerGems = playerProfile.gems;
        GEM.playerStamina = playerProfile.stamina;
        GEM.playerStaminaMaxDateTime = playerProfile.staminaMaxDateTime;
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

    public PlayersProfile(string p_name, string p_farmName, int p_level, int p_XPPoints, int p_coins, int p_gems, int p_stamina, string p_staminaMaxDateTime)
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

    public PlayersProfile()
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
