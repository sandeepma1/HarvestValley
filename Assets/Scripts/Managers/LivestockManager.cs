using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestockManager : Singleton<LivestockManager>
{
    [SerializeField]
    private ClickableLivestock clickableLivestockPrefab;

    private ClickableLivestock[] livestockGO;
    private void Start()
    {
        Init();
    }

    private void OnApplicationQuit()
    {
        SomethingChangedSaveLivestock();
    }

    private void Init()
    {
        List<LivestockClass> livestock = ES2.LoadList<LivestockClass>("AllLivestock");
        livestockGO = new ClickableLivestock[livestock.Count];
        for (int i = 0; i < livestock.Count; i++)
        {
            livestockGO[i] = Instantiate(clickableLivestockPrefab, this.transform);
            livestockGO[i].livestock = livestock[i];
            livestockGO[i].dateTime = DateTime.Parse(livestock[i].dateTime);
            livestockGO[i].SaveLivestock += SaveLivestockEventTrigger;
        }
    }

    private void SaveLivestockEventTrigger()
    {
        SomethingChangedSaveLivestock();
    }

    public void SomethingChangedSaveLivestock()
    {
        SaveLivestock();
    }

    private void SaveLivestock()
    {
        List<LivestockClass> livestock = new List<LivestockClass>();
        for (int i = 0; i < livestockGO.Length; i++)
        {
            livestock.Add(new LivestockClass());
            livestock[i] = livestockGO[i].livestock;
            livestock[i].dateTime = livestockGO[i].dateTime.ToString();
        }
        ES2.Save(livestock, "AllLivestock");
    }
}

[System.Serializable]
public class LivestockClass  // iLIST
{
    public int livestockId;
    public int canProduceItemId;
    public int biteCount;
    public int hatched;
    public int maxHatchCount;
    public LivestockType livestockType;
    public string dateTime;

    public LivestockClass()
    {
    }

    public LivestockClass(int l_livestockId, int l_canProduceItemId, int l_biteCount, int l_hatched, int l_maxHatchCount, LivestockType l_livestockTyped, string l_dateTime)
    {
        livestockId = l_livestockId;
        canProduceItemId = l_canProduceItemId;
        biteCount = l_biteCount;
        hatched = l_hatched;
        maxHatchCount = l_maxHatchCount;
        livestockType = l_livestockTyped;
        dateTime = l_dateTime;
    }
}

public enum LivestockType
{
    Chicken,
    Cow
}

public enum LivestockState
{
    Idle,
    Eating,
    WaitingForHatch
}