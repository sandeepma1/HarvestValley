using System.Collections;
using UnityEngine;
using TMPro;
using System;
using HarvestValley.Ui;
using HarvestValley.IO;

public class PlayerProfileManager : Singleton<PlayerProfileManager>
{
    [SerializeField]
    public TextMeshProUGUI coinsUIText, gemsUIText, staminaUIText, levelUIText, XPPointsUIText;
    public PlayersProfile playerProfile;
    private bool isLevelUpReady;

    protected override void Awake()
    {
        base.Awake();
        if (!ES2.Exists("PlayerProfile"))
        {
            FirstScript.Instance.ResetGame();
        }
        playerProfile = ES2.Load<PlayersProfile>("PlayerProfile");
        InitPlayerProfile();
    }

    private void Start()
    {
        UpdateHudUi();
    }

    public int CurrentPlayerLevel
    {
        get { return playerProfile.level; }
    }

    public int CurrentPlayerXP
    {
        get { return playerProfile.XPPoints; }
    }

    private void LateUpdate()
    {
        if (isLevelUpReady && !Input.anyKey)
        {
            isLevelUpReady = false;
            MenuManager.Instance.DisplayMenu(MenuNames.LevelUp, MenuOpeningType.CloseAll);
        }
    }

    public bool IsCoinsAvailable(int value)
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

    public void CheckForLevelUp()
    {
        if (playerProfile.XPPoints >= LevelUpDatabase.GetLevelById(playerProfile.level).XPforNextLevel)
        {
            IncrementPlayerLevel();
            Level currentUnlockedLevel = LevelUpDatabase.GetLevelById(playerProfile.level);
            if (currentUnlockedLevel.itemUnlockID >= 0)
            {
                UiInventoryMenu.Instance.AddNewItem(currentUnlockedLevel.itemUnlockID, currentUnlockedLevel.itemRewardCount);
                UiSeedListMenu.Instance.AddUnlockedItemsToList();
                UiBuildingMenu.Instance.AddUnlockedItemsToList();
                UiGrassListMenu.Instance.AddUnlockedItemsToList();
                PlayerGems(currentUnlockedLevel.gemsRewardCount);
            }
            PlayerXPPointsAdd(-CurrentPlayerXP);
            isLevelUpReady = true;
        }
        UpdateHudUi();
    }

    public void PlayerName(string value)
    {
        playerProfile.name = value;
        UpdateHudUi();
    }

    public void FarmName(string value)
    {
        playerProfile.farmName = value;
        UpdateHudUi();
    }

    public void PlayerCoins(int value)
    {
        playerProfile.coins += value;
        UpdateHudUi();
    }

    public void PlayerGems(int value)
    {
        playerProfile.gems += value;
        UpdateHudUi();
    }

    public void IncrementPlayerLevel()
    {
        playerProfile.level++;
        UpdateHudUi();
    }

    public void PlayerXPPointsAdd(int value)
    {
        playerProfile.XPPoints += value;
        CheckForLevelUp();
        UpdateHudUi();
    }

    public void PlayerStamina(int value)
    {
        playerProfile.stamina += value;
        UpdateHudUi();
    }

    private void UpdateHudUi()
    {
        coinsUIText.text = String.Format("{0:###,###,###,###,###}", playerProfile.coins);
        gemsUIText.text = String.Format("{0:###,###,###,###,###}", playerProfile.gems);
        staminaUIText.text = playerProfile.stamina.ToString();
        levelUIText.text = playerProfile.level.ToString();
        XPPointsUIText.text = playerProfile.XPPoints.ToString() + "/" + LevelUpDatabase.GetLevelById(playerProfile.level).XPforNextLevel.ToString();
        SavePlayerProfile();
    }

    private void SavePlayerProfile()
    {
        ES2.Save(playerProfile, "PlayerProfile");
    }

    #region Init PlayerProfile

    void InitPlayerProfile()
    {
        GEM.playerName = playerProfile.name;
        GEM.playerFarmName = playerProfile.farmName;
        GEM.playerLevel = playerProfile.level;
        GEM.playerXPPoints = playerProfile.XPPoints;
        GEM.playerCoins = playerProfile.coins;
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
