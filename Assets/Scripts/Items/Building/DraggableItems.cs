using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItems : MonoBehaviour
{

	public int itemID = 0;
	Vector3 intialPosition;

	void OnMouseDown ()
	{
		intialPosition = transform.localPosition;
	}

	void OnMouseDrag ()
	{
		MasterMenuManager.m_instance.ChildCallingOnMouseDrag (itemID);	
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y + 10, 10));
		GetComponent <BoxCollider2D> ().enabled = false;
	}

	void OnMouseUp ()
	{
		print ("OnMouseUp");
		transform.localPosition = intialPosition;
		MasterMenuManager.m_instance.ChildCallingOnMouseUp (itemID);
		GetComponent <BoxCollider2D> ().enabled = true;
	}
}
