
namespace HarvestValley.Managers
{
    public abstract class ManagerBase<T> : Singleton<T> where T : ManagerBase<T>
    {
        internal int x = 6, y = 4, gap = 2;
        internal int currentSelectedBuildingID = -1;
        internal int currentlSelectedSourceID = -1;

        public virtual void OnBuildingClickedEventHandler(int buildingID, int sourceID)
        {
            currentSelectedBuildingID = buildingID;
            currentlSelectedSourceID = sourceID;
        }
    }
}