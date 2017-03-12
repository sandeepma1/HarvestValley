using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropMenuManager : MonoBehaviour
{
	public static CropMenuManager m_instance = null;
	public GameObject[] crops;
	public GameObject[] menuPages;
	public GameObject switchButton, feildInfoButton;
	public bool isSeedSelected = false;
	public int seedSelectedID = -1;
	int a = 0;
	int maxPages = 3;

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		foreach (var item in menuPages) {
			item.SetActive (false);
		}
		menuPages [a].SetActive (true);
	}

	public void ToggleMenuPages ()
	{
		a++;
		if (a > maxPages - 1) {
			a = 0;
		}
		foreach (var item in menuPages) {
			item.SetActive (false);
		}
		menuPages [a].SetActive (true);
	}


	public void ChildCallingOnMouseUp (int id)
	{
		isSeedSelected = false;
		PlacableTileManager.m_instance.plantedOnSelectedFeild = false;
		seedSelectedID = -1;
		switchButton.SetActive (true);
		feildInfoButton.SetActive (true);
	}

	public void ChildCallingOnMouseDrag (int id)
	{
		isSeedSelected = true;
		seedSelectedID = id;
		switchButton.SetActive (false);
		feildInfoButton.SetActive (false);
	}

	public void ToggleDisplayCropMenu ()
	{		
		transform.position = new Vector3 (-500, -500, 0);
	}
}
