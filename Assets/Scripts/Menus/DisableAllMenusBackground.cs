using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAllMenusBackground : MonoBehaviour
{
	public static DisableAllMenusBackground m_instance = null;
	public GameObject[] allMenus;
	//Vector3 screenPoint, scanPos, offset;

	void OnMouseUp ()
	{
		DisableAllMenus ();
	}

	public void DisableAllMenus ()
	{
		for (int i = 0; i < allMenus.Length; i++) {
			allMenus [i].SetActive (false);
		}
	}
}
