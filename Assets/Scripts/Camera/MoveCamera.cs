using UnityEngine;
using System.Collections;


public class MoveCamera : MonoBehaviour
{
	//
	// VARIABLES
	//
	public float minX, maxX, minY, maxY;
	public float turnSpeed = 35.0f;
	// Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 360.0f;
	// Speed of the camera when being panned
	public float zoomSpeed = 500.0f;
	// Speed of the camera going back and forth
	
	public float turnDrag = 5.0f;
	// RigidBody Drag when rotating camera
	public float panDrag = 3.5f;
	// RigidBody Drag when panning camera
	public float zoomDrag = 3.3f;
	// RigidBody Drag when zooming camera
	
	private Vector3 mouseOrigin, pos, move;
	// Position of cursor when mouse dragging starts
	private bool isPanning;
	// Is the camera being panned?
	private bool isRotating;
	// Is the camera being rotated?
	private bool isZooming;
	// Is the camera zooming?
	public Rigidbody rigbody;
	Vector2 camPosition;
	//
	// AWAKE
	//
	
	void Awake ()
	{
		gameObject.AddComponent<Rigidbody> ();
		rigbody.useGravity = false;
	}

	void SetBoundries ()
	{
		transform.Translate (Vector3.one * 1 * Time.deltaTime, Space.Self);
		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -3, 3), Mathf.Clamp (transform.position.y, -3, 3), -200);
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			//isRotating = true;
			isPanning = true;
		}		
		// == Disable movements on Input Release ==		
		if (!Input.GetMouseButton (0)) {			
			isPanning = false;
		}	
		if (isPanning) {	
			pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - mouseOrigin);
			move = new Vector3 (pos.x * panSpeed, pos.y * panSpeed, 0);
			// Apply the pan's move vector in the orientation of the camera's front
			Quaternion forwardRotation = Quaternion.LookRotation (transform.up, transform.forward);
			move = forwardRotation * move;
			// Set Drag
			rigbody.drag = panDrag;			
			// Pan
			rigbody.AddForce (move, ForceMode.Acceleration);
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -3, 3), Mathf.Clamp (transform.position.y, -3, 3), -200);
		}
	}
	
}