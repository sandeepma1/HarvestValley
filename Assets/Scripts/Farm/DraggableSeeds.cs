using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSeeds : MonoBehaviour
{
	public int seedID = 0;
	Vector3 intialPosition;

	void OnMouseDown ()
	{
		intialPosition = transform.position;
	}

	void OnMouseDrag ()
	{
		GameEventManager.isSeedSelected = true;
		GameEventManager.seedSelectedID = seedID;
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y + 10, 10));
		GetComponent <BoxCollider2D> ().enabled = false;
		//transform.parent.gameObject.SetActive (false);
	}

	void OnMouseUp ()
	{		
		transform.position = intialPosition;
		GameEventManager.isSeedSelected = false;
		GameEventManager.seedSelectedID = -1;
		GetComponent <BoxCollider2D> ().enabled = true;
		//transform.parent.gameObject.SetActive (true);
	}
}
