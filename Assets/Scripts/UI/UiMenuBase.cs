using System;
using System.Collections.Generic;
using UnityEngine.UI;
using HarvestValley.IO;
using UnityEngine;

namespace HarvestValley.Ui
{
    public abstract class UiMenuBase<T> : Singleton<T> where T : UiMenuBase<T>
    {
        internal List<int> allUnlockedItemIDs = new List<int>();
        internal int selectedBuildingID = -1;
        internal int selectedSourceID = -1;

        protected virtual void Start()
        {
            CreateCloseButton();
        }

        /// <summary>
        /// Setting Close button and background color of every menu inherated from this.
        /// </summary>
        private void CreateCloseButton()
        {
            Transform backGroundButton = transform.GetChild(0).GetChild(0);
            if (backGroundButton.GetComponent<Button>())
            {
                backGroundButton.GetComponent<Button>().onClick.AddListener(MenuManager.Instance.CloseMenu);
                backGroundButton.GetComponent<Image>().color = ColorConstants.CloseButtonBackground;
            }

            //Assign event to close for X close button
            if (transform.GetChild(0).GetChild(1).childCount > 1)
            {
                Transform closeGroundButton = transform.GetChild(0).GetChild(1).GetChild(0);
                if (closeGroundButton.GetComponent<Button>())
                {
                    closeGroundButton.GetComponent<Button>().onClick.AddListener(MenuManager.Instance.CloseMenu);
                    closeGroundButton.GetComponent<Image>().color = ColorConstants.NormalUiItem;
                }
            }
        }

        public virtual void AddUnlockedItemsToList()  // call on level change & game start only
        {
            for (int i = 0; i <= PlayerProfileManager.Instance.CurrentPlayerLevel; i++)
            {
                int unlockedId = LevelUpDatabase.GetLevelById(i).itemUnlockID;

                if (unlockedId >= 0 && !allUnlockedItemIDs.Contains(unlockedId))
                {
                    allUnlockedItemIDs.Add(unlockedId);
                }
            }
        }

        protected string TimeRemaining(DateTime dateTime)
        {
            TimeSpan timeSpan;
            timeSpan = dateTime.Subtract(DateTime.UtcNow);
            return TimeSpanToDuration(timeSpan);
        }

        protected string SecondsToDuration(int seconds)
        {
            return TimeSpanToDuration(new TimeSpan(0, 0, seconds));
        }

        private string TimeSpanToDuration(TimeSpan timeSpan)
        {
            string timeRemaining = "";
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