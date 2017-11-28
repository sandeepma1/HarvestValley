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
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y + 30, 10));
		MasterMenuManager.Instance.ChildCallingOnMouseDown (itemID, transform.localPosition);	
		GetComponent <BoxCollider2D> ().enabled = false;
	}

	void OnMouseDrag ()
	{
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y + 30, 10));
		MasterMenuManager.Instance.ChildCallingOnMouseDrag (itemID, transform.localPosition);	
		GetComponent <BoxCollider2D> ().enabled = false;
	}

	void OnMouseUp ()
	{
		transform.localPosition = intialPosition;
		MasterMenuManager.Instance.ChildCallingOnMouseUp (itemID);
		GetComponent <BoxCollider2D> ().enabled = true;
	}
}
