using System;
using System.Collections;
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
            for (int i = 0; i <= PlayerProfileManager.Instance.CurrentPlayerLevel; i++)
            {
                int unlockedId = LevelUpDatabase.Instance.gameLevels[i].itemUnlockID;

                if (unlockedId >= 0)
                {
                    unlockedItemIDs.Add(unlockedId);
                }
            }
            UpdateSeedItems();
            PopulateBuildingItems();
        }

        public virtual void UpdateSeedItems()
        { }

        public virtual void PopulateBuildingItems()
        { }

        protected string TimeRemaining(DateTime dateTime)
        {
            string timeRemaining = "";
            TimeSpan timeSpan;
            timeSpan = dateTime.Subtract(DateTime.UtcNow);

            if (timeSpan <= new TimeSpan(360, 0, 0, 0))
            { //> 1year
                timeRemaining = timeSpan.Days.ToString() + "d " + timeSpan.Hours.ToString() + "h";
            }
            if (timeSpan <= new TimeSpan(1, 0, 0, 0))
            { //> 1day
                timeRemaining = timeSpan.Hours.ToString() + "h " + timeSpan.Minutes.ToString() + "m";
            }
            if (timeSpan <= new TimeSpan(0, 1, 0, 0))
            { //> 1hr
                timeRemaining = timeSpan.Minutes.ToString() + "m " + timeSpan.Seconds.ToString() + "s";
            }
            if (timeSpan <= new TimeSpan(0, 0, 1, 0))
            { // 1min
                timeRemaining = timeSpan.Seconds.ToString() + "s";
            }
            if (timeSpan <= new TimeSpan(0, 0, 0, 0))
            {
                timeRemaining = "";
            }
            return timeRemaining;
        }
    }
}