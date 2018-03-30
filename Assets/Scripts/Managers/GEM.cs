using UnityEngine;
using System;
using System.Collections;

public static class GEM
{
    #region  Player Profile
    public static string playerName;
    public static string playerFarmName;
    public static int playerLevel;
    public static int playerXPPoints;
    public static int playerCoins;
    public static int playerGems;
    public static int playerStamina;
    public static string playerStaminaMaxDateTime;
    #endregion

    #region Debug Stuff
    public static bool ShowDebugInfo = false;
    #endregion

    #region Building Variables
    public static int maxBuildingQueueCount = 9;
    public static int maxRequiredItems = 4;
    #endregion

    public static int numberOfRocksInLevel = 0;
    //public static bool isSeedSelected = false;
    //public static int seedSelectedID = -1;
    public static int[] screensPositions = new int[] { 0, 7, 24, 41, 58, 75, 92 };
    public static int[] farmLandUpgrade = new int[] { 1, 3, 5, 7, 9, 11, 13, 16 };
    public static bool isObjectDragging = false;

    public delegate void GameEvent();

    static E_STATES m_gameState = E_STATES.e_game;
    public static void SetState(E_STATES state)
    {
        m_gameState = state;
    }
    public static E_STATES GetState()
    {
        return m_gameState;
    }
    public enum E_STATES
    {
        e_game,
        e_pause,
        e_inventory
    };

    public static System.DateTime dateTime = System.DateTime.UtcNow;

    static E_MenuState m_menuState = E_MenuState.e_menuDown;
    public static void SetMenuState(E_MenuState state)
    {
        m_menuState = state;
    }
    public static E_MenuState GetMenuState()
    {
        return m_menuState;
    }
    public enum E_MenuState
    {
        e_menuUp,
        e_menuDown
    };


    static E_PlayerTerrianSTATES m_playerTerrianState = E_PlayerTerrianSTATES.land;
    public static void SetPlayerTerrianSTATES(E_PlayerTerrianSTATES state)
    {
        m_playerTerrianState = state;
    }
    public static E_PlayerTerrianSTATES GetPlayerTerrianSTATES()
    {
        return m_playerTerrianState;
    }
    public enum E_PlayerTerrianSTATES
    {
        deepwater,
        water,
        sand,
        land,
        stone
    };
}

[SerializeField]
public struct item
{
    public sbyte id;
    public sbyte age;
    public GameObject GO;
}

[SerializeField]
public enum onHarvest
{
    //Carrots
    Destory,
    // Trees
    RegrowToStump,
    // Berries
    Renewable
}


