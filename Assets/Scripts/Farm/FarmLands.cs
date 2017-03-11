using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FarmLands : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler
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

	public void OnPointerDown (PointerEventData eventData)//*
	{	
		PlacableTileManager.m_instance.CallParentOnMouseDown (objectID);
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		PlacableTileManager.m_instance.CallParentOnMouseEnter (objectID);
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		PlacableTileManager.m_instance.CallParentOnMouseUp (objectID);
	}

	/*public void OnMouseDrag (PointerEventData eventData)//*
	{
		PlacableTileManager.m_instance.CallParentOnMouseDrag (objectID);
	}*/

	public void OnPointerExit (PointerEventData eventData)//*
	{
		PlacableTileManager.m_instance.CallParentOnMouseExit (objectID);
	}

	/*void OnMouseUpAsButton (PointerEventData eventData)
	{
		PlacableTileManager.m_instance.CallParentOnMouseUpAsButton (objectID);
	}*/

}

