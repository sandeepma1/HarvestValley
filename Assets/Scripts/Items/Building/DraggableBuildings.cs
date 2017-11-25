using UnityEngine;

public class DraggableBuildings : MonoBehaviour
{
    public bool isSelected = false;
    public bool isDraggable = false;
    public int id;
    public int buildingID;
    public Vector2 pos;
    public int level;
    public BUILDINGS_STATE state;
    public int unlockedQueueSlots;
    public int itemID;
    public System.DateTime dateTime;
    //public string s_dateTime = "";

    public void OnClickDrag()
    {
        BuildingsManager.m_instance.CallParentOnMouseDrag(id);
    }

    public void OnClickUp()
    {
        BuildingsManager.m_instance.CallParentOnMouseUp(id);
        GEM.isSwipeEnable = true;
    }

    public void OnClickEnter()
    {
        GEM.isSwipeEnable = false;
        BuildingsManager.m_instance.CallParentOnMouseEnter(id);
    }

    public void OnMouseDown()
    {
        GEM.isSwipeEnable = false;
        BuildingsManager.m_instance.CallParentOnMouseDown(id);
    }

    #region Unused

    /*public void OnMouseUp ()
	{
		BuildingsManager.m_instance.CallParentOnMouseUp (id);
	}*/

    /*	public void OnMouseEnter ()
	{
		BuildingsManager.m_instance.CallParentOnMouseEnter (id);
	}*/

    public void OnClickDown()//* IPointerDownHandler  //** not using
    {
        //PlacableTileManager.m_instance.CallParentOnMouseDown (id);
    }

    public void OnClickExit() //** not using
    {
        //PlacableTileManager.m_instance.CallParentOnMouseExit (id);
    }

    #endregion
}

