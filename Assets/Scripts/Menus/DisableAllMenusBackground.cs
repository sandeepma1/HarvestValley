using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAllMenusBackground : MonoBehaviour
{
	public GameObject[] allMenus;
	//Vector3 screenPoint, scanPos, offset;

	void OnMouseUp ()
	{
		for (int i = 0; i < allMenus.Length; i++) {
			allMenus [i].SetActive (false);
		}
		print ("last layer clicked");

		/*	screenPoint = Camera.main.WorldToScreenPoint (scanPos);
		offset = scanPos - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));*/
	}

	void OnMouseDrag ()
	{
		/*Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
		transform.position = curPosition;*/
	}
}
