using System.Collections.Generic;
using UnityEngine;

namespace HarvestValley.Ui
{
    public abstract class BuildingMenuBase<T> : Singleton<T> where T : BuildingMenuBase<T>
    {
        internal List<int> unlockedItemIDs = new List<int>();
        internal int selectedBuildingID = -1;
        internal int selectedSourceID = -1;

        public virtual void Start()
        {
            AddUnlockedItemsToList();
        }

        public virtual void AddUnlockedItemsToList()  // call on level change & game start only
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
            print("Checked all unlocked items " + unlockedItemIDs.Count);
            UpdateSeedItems();
        }

        public virtual void UpdateSeedItems()
        { }
    }
}