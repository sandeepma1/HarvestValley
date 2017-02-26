using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAllMenusBackground : MonoBehaviour
{
	public GameObject[] allMenus;


	void OnMouseDown ()
	{
		for (int i = 0; i < allMenus.Length; i++) {
			allMenus [i].SetActive (false);
		}
		print ("last layer clicked");
	}
}
