using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBuildings  : MonoBehaviour
{
	public bool isSelected = false;
	public int id;
	public int buildingID;
	public Vector2 pos;
	public int level;
	public BUILDINGS_STATE state;
	public int itemID1;
	public System.DateTime dateTime1;
	public int itemID2;
	public System.DateTime dateTime2;
	public int itemID3;
	public System.DateTime dateTime3;
	public string s_dateTime = "";
	Vector3 poss = Vector3.zero;
	//TODO remove the s_dateTime variable

	void Start ()
	{
		s_dateTime = dateTime1.ToString ();
	}

	public void OnClickDrag ()
	{
		print (id);
		BuildingsManager.m_instance.CallParentOnMouseDrag (id);
	}

	public void OnClickUp ()
	{
		BuildingsManager.m_instance.CallParentOnMouseUp (id);
	}

	/*public void OnMouseUp ()
	{
		BuildingsManager.m_instance.CallParentOnMouseUp (id);
	}*/

	/*	public void OnMouseEnter ()
	{
		BuildingsManager.m_instance.CallParentOnMouseEnter (id);
	}*/

	/*	public void OnMouseDown ()
	{
		BuildingsManager.m_instance.CallParentOnMouseDown (id);
	}*/

	public void OnClickExit () //** not using
	{
		//PlacableTileManager.m_instance.CallParentOnMouseExit (id);
	}

	public void OnClickEnter ()
	{
		
		BuildingsManager.m_instance.CallParentOnMouseEnter (id);
	}

	public void OnClickDown ()//* IPointerDownHandler  //** not using
	{	
		//PlacableTileManager.m_instance.CallParentOnMouseDown (id);
	}
}

