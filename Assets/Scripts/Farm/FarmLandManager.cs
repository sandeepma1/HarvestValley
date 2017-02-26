using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLandManager : MonoBehaviour
{
	public GameObject FarmLandMenu = null;
	public float longPressTime = 0.5f;
	float time = 0;
	bool isPressed = false;
	bool isLongPressed = false;


	void OnMouseDown ()
	{		
		print ("Down");
		isPressed = true;
	}

	void OnMouseUp ()
	{
		if (!isLongPressed) {
			FarmLandMenu.transform.position = transform.position;
			FarmLandMenu.SetActive (true);

		}
		time = 0;
		GetComponent <SpriteRenderer> ().color = Color.white;
		isPressed = false;
		isLongPressed = false;
	}

	void Update ()
	{
		if (isPressed) {
			if (time >= longPressTime) {
				GetComponent <SpriteRenderer> ().color = Color.red;
				isLongPressed = true;
				return;
			}
			time += Time.deltaTime;
		}
	}
}
