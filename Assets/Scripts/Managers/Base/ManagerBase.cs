
public abstract class ManagerBase<T> : Singleton<T> where T : ManagerBase<T>
{
    internal int x = 5, y = 5, gap;
    internal int currentSelectedBuildingID = -1;
    internal int currentlSelectedSourceID = -1;

    public virtual void OnBuildingClickedEventHandler(int buildingID, int sourceID)
    {
        currentSelectedBuildingID = buildingID;
        currentlSelectedSourceID = sourceID;
    }
}