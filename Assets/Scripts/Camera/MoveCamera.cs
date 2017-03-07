using UnityEngine;
using System.Collections;


public class MoveCamera : MonoBehaviour
{
	//
	// VARIABLES
	//
	//public float minX, maxX, minY, maxY;
	public float panSpeed = 360.0f;
	public float panDrag = 3.5f;
	private Vector3 mouseOrigin, pos, posTemp, move;
	// Position of cursor when mouse dragging starts
	private bool isPanning;
	public Rigidbody rigbody;
	Vector2 camPosition;

	Hashtable ease = new Hashtable ();
	float originalPoint = 0;
	float originalPointRight = 0;
	float originalPointLeft = 0;
	float buffer = 2;
	float screenSize = 10;

	void Awake ()
	{
		originalPoint = screenSize;
		originalPointRight = originalPoint + buffer;
		originalPointLeft = originalPoint - buffer;

		ease.Add ("ease", LeanTweenType.linear);
		gameObject.AddComponent<Rigidbody> ();
		rigbody.useGravity = false;
	}

	/*	void SetBoundries ()
	{
		transform.Translate (Vector3.one * 1 * Time.deltaTime, Space.Self);
		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -3, 3), Mathf.Clamp (transform.position.y, -3, 3), -200);
	}*/

	void Update ()
	{
		
		if (Input.GetMouseButtonDown (0)) {
			mouseOrigin = Input.mousePosition;
			isPanning = true;
		}
		if (!Input.GetMouseButton (0)) {			
			isPanning = false;
		}


		if (isPanning) {
			posTemp = pos;
			pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - mouseOrigin);
			DebugTextHandler.m_instance.DisplayDebugText (pos.ToString () + " " + posTemp.ToString ());	

			if (posTemp == pos) {
				return;
			}

			move = new Vector3 (-pos.x * panSpeed, -pos.y * panSpeed, -10);
			rigbody.drag = panDrag;			
			rigbody.AddForce (move, ForceMode.Acceleration);
		}
	}

	void LateUpdate ()
	{
		if (transform.position.y > 0) {
			transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
		}

	}
}