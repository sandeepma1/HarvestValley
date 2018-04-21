using UnityEngine;

namespace HarvestValley.Managers
{
    public abstract class ManagerBase<T> : Singleton<T> where T : ManagerBase<T>
    {
        internal int currentSelectedBuildingID = -1;
        internal int currentlSelectedSourceID = -1;

        public virtual void OnBuildingClicked(int buildingID, int sourceID)
        {
            currentSelectedBuildingID = buildingID;
            currentlSelectedSourceID = sourceID;
        }
    }
}