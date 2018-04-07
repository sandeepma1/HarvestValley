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

    private void Init()
    {
        List<LivestockClass> livestock = ES2.LoadList<LivestockClass>("AllLivestock");
        livestockGO = new ClickableLivestock[livestock.Count];
        for (int i = 0; i < livestock.Count; i++)
        {
            livestockGO[i] = Instantiate(clickableLivestockPrefab, this.transform);
            livestockGO[i].livestock = livestock[i];
        }
    }

    public void SaveLivestock()
    {
        List<LivestockClass> livestock = new List<LivestockClass>();
        for (int i = 0; i < livestockGO.Length; i++)
        {
            livestock.Add(new LivestockClass());
            livestock[i] = livestockGO[i].livestock;
        }
        ES2.Save(livestock, "AllLivestock");
    }
}

[System.Serializable]
public class LivestockClass  // iLIST
{
    public int sourceId;
    public int canProduceItemId;
    public int biteCount;
    public int hatched;
    public int maxHatchCount;
    public string dateTime;

    public LivestockClass()
    {
    }

    public LivestockClass(int l_sourceId, int l_canProduceItemId, int l_biteCount, int l_hatched, int l_maxHatchCount, string l_dateTime)
    {
        sourceId = l_sourceId;
        canProduceItemId = l_canProduceItemId;
        biteCount = l_biteCount;
        hatched = l_hatched;
        maxHatchCount = l_maxHatchCount;
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