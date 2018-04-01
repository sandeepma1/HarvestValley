using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestockManager : MonoBehaviour
{
    [SerializeField]
    private ClickableLivestock clickableLivestockPrefab;

    private List<LivestockClass> livestock = new List<LivestockClass>();
    private ClickableLivestock[] livestockGO;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        livestock = ES2.LoadList<LivestockClass>("AllLivestock");
        livestockGO = new ClickableLivestock[livestock.Count];
        for (int i = 0; i < livestock.Count; i++)
        {
            livestockGO[i] = Instantiate(clickableLivestockPrefab, this.transform);
            livestockGO[i].livestock = livestock[i];
            livestockGO[i].dateTime = DateTime.Parse(livestock[i].dateTime);
        }
    }

    private void SaveLivestock()
    {
        for (int i = 0; i < livestock.Count; i++)
        {
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
    public int alreadyAteGrassCount;
    public LivestockType livestockType;
    public LivestockState livestockState;
    public string dateTime;

    public LivestockClass()
    {
    }

    public LivestockClass(int l_livestockId, int l_canProduceItemId, int l_alreadyAteGrassCount, LivestockType l_livestockTyped, LivestockState l_livestockState, string l_dateTime)
    {
        livestockId = l_livestockId;
        canProduceItemId = l_canProduceItemId;
        alreadyAteGrassCount = l_alreadyAteGrassCount;
        livestockType = l_livestockTyped;
        livestockState = l_livestockState;
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
    WaitingForGrass,
    WaitingForHatch
}