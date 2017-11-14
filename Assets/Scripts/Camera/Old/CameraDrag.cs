using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

	public float dragSpeed = 2.0f;
	private Vector3 dragOrigin;
	private bool isPanning;
	private Vector3 velocity = Vector3.zero;
     
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			dragOrigin = Input.mousePosition;
			isPanning = true;
		}
 
		if (!Input.GetMouseButton (0)) {
			isPanning = false;
		}
 
		if (isPanning) {
			Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);
 
			Vector3 move = new Vector3 (pos.x * dragSpeed, pos.y * dragSpeed, 0);
			Quaternion forwardRotation = Quaternion.LookRotation (transform.up, transform.forward);
			move = forwardRotation * move;
			transform.Translate (move, Space.Self);
			//transform.position = Vector3.SmoothDamp (new Vector3 (transform.position.x, transform.position.y, -200), pos, ref velocity, 0.3f);
 
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -3, 3), Mathf.Clamp (transform.position.y, -3, 3), -200);  //without this the code works fine but I want to set boundaries for the actual map.
		}
	}
}