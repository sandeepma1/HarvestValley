using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingMenuBase<T> : Singleton<T> where T : BuildingMenuBase<T>
{
    internal List<int> unlockedItemIDs = new List<int>();
    internal int selectedBuildingID = -1;
    internal int selectedSourceID = -1;

    public virtual void Start()
    {
        CheckForUnlockedItems();
    }

    public virtual void OnEnable()
    {
        if (selectedBuildingID == -1 || selectedSourceID == -1)
        {
            if (GEM.ShowDebugInfo) Debug.LogWarning("Selected field/building is -1");
            return;
        }
    }

    public virtual void CheckForUnlockedItems()  // call on level change & game start only
    {
        unlockedItemIDs.Clear();
        for (int i = 0; i <= PlayerProfileManager.Instance.CurrentPlayerLevel(); i++)
        {
            int unlockedId = LevelUpDatabase.Instance.gameLevels[i].itemUnlockID;

            if (unlockedId >= 0)
            {
                unlockedItemIDs.Add(unlockedId);
            }
        }
    }
}

