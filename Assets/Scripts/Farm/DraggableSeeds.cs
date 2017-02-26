using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableSeeds : MonoBehaviour
{
	Vector3 intialPosition;

	void OnMouseDown ()
	{
		intialPosition = transform.position;
	}

	void OnMouseDrag ()
	{
		print ("Dragging");
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y + 10, 10));
	}

	void OnMouseUp ()
	{		
		transform.position = intialPosition;
	}
}
