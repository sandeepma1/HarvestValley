using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FarmLands : MonoBehaviour
{
	int objectID;
	public int id;
	public Vector2 pos;
	public int level;
	public int seedID;
	public FARM_LAND_STATE state;
	public System.DateTime dateTime;
	public string s_dateTime = "";
	//TODO remove the s_dateTime variable

	void Start ()
	{
		string s = gameObject.name.Replace ("FarmLand", "");
		int.TryParse (s, out objectID);
		s_dateTime = dateTime.ToString ();
	}

	void OnMouseDown ()
	{	
		PlacableTileManager.m_instance.CallParentOnMouseDown (objectID);
	}

	void OnMouseEnter ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseEnter (objectID);
	}

	void OnMouseUp ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseUp (objectID);
	}

	void OnMouseDrag ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseDrag (objectID);
	}

	void OnMouseExit ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseExit (objectID);
	}

	void OnMouseUpAsButton ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseUpAsButton (objectID);
	}

}

