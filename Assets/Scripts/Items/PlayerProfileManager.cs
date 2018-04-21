using System.Collections;
using UnityEngine;
using TMPro;
using System;
using HarvestValley.Ui;
using HarvestValley.IO;
using UnityEngine.UI;

public class PlayerProfileManager : Singleton<PlayerProfileManager>
{
    [SerializeField]
    private TextMeshProUGUI coinsUIText;
    [SerializeField]
    private TextMeshProUGUI gemsUIText;
    [SerializeField]
    private TextMeshProUGUI staminaUIText;
    [SerializeField]
    private TextMeshProUGUI levelUIText;
    [SerializeField]
    private TextMeshProUGUI XPPointsUIText;
    [SerializeField]
    private Transform XpProgressBar;

    public PlayersProfile playerProfile;
    private bool isLevelUpReady;

    [SerializeField]
    private Button CheatLevelAdd;

    protected override void Awake()
    {
        base.Awake();
        playerProfile = ES2.Load<PlayersProfile>("PlayerProfile");
        InitPlayerProfile();
    }

    private void Start()
    {
        UpdateHudUi();
        CheatLevelAdd.onClick.AddListener(AddXP);
    }

    private void AddXP()
    {
        PlayerXPPointsAdd(CurrentPlayerLevel * 50);
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
                UiInventoryMenu.Instance.UpdateItems(currentUnlockedLevel.itemUnlockID, currentUnlockedLevel.itemRewardCount);
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
        // XPPointsUIText.text = playerProfile.XPPoints.ToString() + "/" + LevelUpDatabase.GetLevelById(playerProfile.level).XPforNextLevel.ToString();

        float differenceInXp = playerProfile.XPPoints / (float)LevelUpDatabase.GetLevelById(playerProfile.level).XPforNextLevel;
        XpProgressBar.localScale = new Vector3(differenceInXp, XpProgressBar.localScale.y, XpProgressBar.localScale.z);
        float percentage = differenceInXp * 100f;
        XPPointsUIText.text = percentage.ToString("F0") + "%";
    }

    public void SavePlayerProfile()
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
