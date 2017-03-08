using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FarmLands : MonoBehaviour
{
	int index;
	public sbyte tileIndex;
	public sbyte level;
	public sbyte seedIndex;
	public sbyte state;
	public System.DateTime dateTime;

	void Start ()
	{
		print (dateTime);
		string s = gameObject.name.Replace ("FarmLand", "");
		int.TryParse (s, out index);
	}

	void OnMouseDown ()
	{	
		PlacableTileManager.m_instance.CallParentOnMouseDown (index);
	}

	void OnMouseEnter ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseEnter (index);
	}

	void OnMouseUp ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseUp (index);
	}

	void OnMouseDrag ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseDrag (index);
	}

	void OnMouseExit ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseExit (index);
	}

	void OnMouseUpAsButton ()
	{
		PlacableTileManager.m_instance.CallParentOnMouseUpAsButton (index);
	}

}

