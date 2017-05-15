using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableBuildings  : MonoBehaviour
{
	public bool isSelected = false;
	public int id;
	public int buildingID;
	public Vector2 pos;
	public int level;
	public BUILDINGS_STATE state;
	public int unlockedQueueSlots;
	public int itemID;
	public System.DateTime dateTime;
	//public string s_dateTime = "";

	public void OnClickDrag ()
	{
		BuildingsManager.m_instance.CallParentOnMouseDrag (id);
	}

	public void OnClickUp ()
	{		
		BuildingsManager.m_instance.CallParentOnMouseUp (id);	
	}

	public void OnClickEnter ()
	{		
		BuildingsManager.m_instance.CallParentOnMouseEnter (id);	
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

	public void OnMouseDown ()
	{
		BuildingsManager.m_instance.CallParentOnMouseDown (id);
	}

	public void OnClickDown ()//* IPointerDownHandler  //** not using
	{	
		//PlacableTileManager.m_instance.CallParentOnMouseDown (id);
	}

	public void OnClickExit () //** not using
	{
		//PlacableTileManager.m_instance.CallParentOnMouseExit (id);
	}

	#endregion
}

