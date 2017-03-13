﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using TMPro;
using UnityEngine.EventSystems;

public class PlacableTileManager : MonoBehaviour
{
	public static PlacableTileManager m_instance = null;
	public GameObject farmLandPrefab;
	public GameObject CropMenu = null, FarmTimerText = null, FarmHarvestingMenu = null;
	public bool isFarmTimerEnabled = false;
	public List<SaveFarmLand> farmList = new List<SaveFarmLand> ();
	GameObject[] FarmLands;
	System.TimeSpan remainingTime;
	int tempID = -1;

	public bool plantedOnSelectedFeild = false;
	public int feildSelectedID = -1;


	void Awake ()
	{
		m_instance = this;
		OneTimeOnly ();
		Init ();
	}

	void Init ()
	{
		farmList = ES2.LoadList<SaveFarmLand> ("farmList");
		FarmLands = new GameObject[farmList.Count];
		foreach (var farm in farmList) {
			InitFarmLands (farm);
		}
		InvokeRepeating ("SaveFarmLands", 0, 5);
	}

	public void AddNewFarmLandInMap (int id)
	{
		print (System.DateTime.UtcNow.ToString ());
		//farmList.Add (new SaveFarmLand (id, 1, 0, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
		GameObject FarmLand = Instantiate (farmLandPrefab, this.transform);
		FarmLand.transform.localPosition = new Vector3 (id, 0, 0);
		FarmLand.gameObject.name = "FarmLand" + id;
		ES2.Save (farmList, "farmList");
	}

	public void ShowFarmLandMenu (int id) // Display Feild Crop Menu
	{
		IGMMenu.m_instance.DisableAllMenus ();
		feildSelectedID = id;
		CropMenu.transform.position = FarmLands [id].transform.position;
		CropMenu.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		CropMenu.SetActive (true);
		LeanTween.scale (CropMenu, Vector3.one, 0.2f, IGMMenu.m_instance.ease);
	}

	public void ShowReadyToHarvestMenu (int id) // Display Harvesting Menu
	{
		IGMMenu.m_instance.DisableAllMenus ();
		FarmHarvestingMenu.transform.position = FarmLands [id].transform.position;
		FarmHarvestingMenu.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		FarmHarvestingMenu.SetActive (true);
		LeanTween.scale (FarmHarvestingMenu, Vector3.one, 0.2f, IGMMenu.m_instance.ease);		
	}

	public void PlantSeedsOnFarmLand (int id) // Planting Seeds
	{		
		if (CropMenuManager.m_instance.isSeedSelected == true) {	
			if (feildSelectedID == id || plantedOnSelectedFeild) {				
				FarmLands [id].GetComponent <FarmLands> ().state = FARM_LAND_STATE.GROWING;
				FarmLands [id].GetComponent <FarmLands> ().seedID = CropMenuManager.m_instance.seedSelectedID;
				FarmLands [id].GetComponent <FarmLands> ().dateTime = UTC.time.liveDateTime.AddMinutes (SeedDatabase.m_instance.seeds [CropMenuManager.m_instance.seedSelectedID].minsToGrow);
				FarmLands [id].GetComponent <SpriteRenderer> ().color = Color.green;
				SaveFarmLands ();
				plantedOnSelectedFeild = true;
				feildSelectedID = -1;
				CropMenuManager.m_instance.ToggleDisplayCropMenu ();	
			}
		}
	}

	public void HarvestCropOnFarmLand (int id) // Harvesting Seeds
	{
		if (HarvestMenuManager.m_instance.isScytheSelected == true) {
			// TODO Heavy update required for Feild Level Based cals*******************
			// only 2 items are added in storage
			print (FarmLands [id].GetComponent <FarmLands> ().seedID);
			FarmItemsManager.m_instance.AddFarmItems (FarmLands [id].GetComponent <FarmLands> ().seedID, 2);
			PlayerProfileManager.m_instance.PlayerXPPoints (ItemDatabase.m_instance.items [FarmLands [id].GetComponent <FarmLands> ().seedID].XP);
			FarmLands [id].GetComponent <FarmLands> ().state = FARM_LAND_STATE.NONE;
			FarmLands [id].GetComponent <FarmLands> ().dateTime = new System.DateTime ();
			FarmLands [id].GetComponent <SpriteRenderer> ().color = Color.white;
			HarvestMenuManager.m_instance.ToggleDisplayHarvestingMenu ();
		}
	}

	public void ShowFarmLandTimeRemaining ()
	{
		FarmTimerText.transform.position = FarmLands [tempID].transform.position;
		remainingTime = FarmLands [tempID].GetComponent <FarmLands> ().dateTime.Subtract (UTC.time.liveDateTime);
		if (remainingTime <= new System.TimeSpan (360, 0, 0, 0)) { //> 1year
			FarmTimerText.transform.GetChild (1).GetComponent <TextMeshPro> ().text = remainingTime.Days.ToString () + "d " + remainingTime.Hours.ToString () + "h";
		}
		if (remainingTime <= new System.TimeSpan (1, 0, 0, 0)) { //> 1day
			FarmTimerText.transform.GetChild (1).GetComponent <TextMeshPro> ().text = remainingTime.Hours.ToString () + "h " + remainingTime.Minutes.ToString () + "m";
		}
		if (remainingTime <= new System.TimeSpan (0, 1, 0, 0)) { //> 1hr
			FarmTimerText.transform.GetChild (1).GetComponent <TextMeshPro> ().text = remainingTime.Minutes.ToString () + "m " + remainingTime.Seconds.ToString () + "s";
		}
		if (remainingTime <= new System.TimeSpan (0, 0, 1, 0)) { // 1min
			FarmTimerText.transform.GetChild (1).GetComponent <TextMeshPro> ().text = remainingTime.Seconds.ToString () + "s";
		}
		if (remainingTime <= new System.TimeSpan (0, 0, 0, 0)) { // 1min
			FarmTimerText.SetActive (false);
		}
	}

	void LateUpdate ()
	{
		if (isFarmTimerEnabled) {			
			ShowFarmLandTimeRemaining ();
		} else {
			tempID = -1;
		}

		foreach (var item in FarmLands) {
			if (item.GetComponent <FarmLands> ().state == FARM_LAND_STATE.GROWING && item.GetComponent <FarmLands> ().dateTime.Subtract (UTC.time.liveDateTime) <= new System.TimeSpan (0, 0, 0)) {				
				item.GetComponent <FarmLands> ().state = FARM_LAND_STATE.WAITING_FOR_HARVEST;
				item.GetComponent <FarmLands> ().dateTime = new System.DateTime ();
				item.GetComponent <SpriteRenderer> ().color = Color.red;
			}
		}
	}

	#region OnMouse Functions

	void OneTimeOnly ()
	{ 
		if (PlayerPrefs.GetInt ("firstFarms") <= 0) {			
			ES2.Delete ("farmList");
			farmList.Add (new SaveFarmLand (0, new Vector2 (0, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (1, new Vector2 (1, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (2, new Vector2 (2, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (3, new Vector2 (3, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (4, new Vector2 (4, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (5, new Vector2 (0, -1), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			ES2.Save (farmList, "farmList");
			PlayerPrefs.SetInt ("firstFarms", 1);
			print (PlayerPrefs.GetInt ("firstFarms") + " only once");
		}
	}

	public void InitFarmLands (SaveFarmLand farm)
	{
		FarmLands [farm.id] = Instantiate (farmLandPrefab, this.transform);
		FarmLands [farm.id].transform.localPosition = farm.pos;
		FarmLands [farm.id].gameObject.name = "FarmLand" + farm.id;
		FarmLands [farm.id].GetComponent <FarmLands> ().id = farm.id;
		FarmLands [farm.id].GetComponent <FarmLands> ().pos = farm.pos;
		FarmLands [farm.id].GetComponent <FarmLands> ().level = farm.level;
		FarmLands [farm.id].GetComponent <FarmLands> ().seedID = farm.seedID;
		FarmLands [farm.id].GetComponent <FarmLands> ().state = (FARM_LAND_STATE)farm.state;
		switch (FarmLands [farm.id].GetComponent <FarmLands> ().state) {
			case FARM_LAND_STATE.NONE:
				FarmLands [farm.id].GetComponent<SpriteRenderer> ().color = Color.white;
				break;
			case FARM_LAND_STATE.GROWING:				
				FarmLands [farm.id].GetComponent<SpriteRenderer> ().color = Color.green;
				break;
			case FARM_LAND_STATE.WAITING_FOR_HARVEST:
				FarmLands [farm.id].GetComponent<SpriteRenderer> ().color = Color.red;
				break;
			default:
				break;
		}
		FarmLands [farm.id].GetComponent <FarmLands> ().dateTime = System.DateTime.Parse (farm.dateTime);
	}

	public void CallParentOnMouseUp (int id)
	{
		switch (FarmLands [id].GetComponent <FarmLands> ().state) {
			case FARM_LAND_STATE.NONE:
				ShowFarmLandMenu (id);
				break;
			case FARM_LAND_STATE.GROWING:
				tempID = id;
				IGMMenu.m_instance.DisableAllMenus ();
				FarmTimerText.SetActive (true);
				isFarmTimerEnabled = true;
				FarmTimerText.transform.GetChild (0).GetComponent <TextMeshPro> ().text = SeedDatabase.m_instance.seeds [FarmLands [tempID].GetComponent <FarmLands> ().seedID].name.ToString ();
				break;
			case FARM_LAND_STATE.WAITING_FOR_HARVEST:
				ShowReadyToHarvestMenu (id);
				break;
			default:
				break;
		}
	}

	public void CallParentOnMouseEnter (int id)
	{
		switch (FarmLands [id].GetComponent <FarmLands> ().state) {
			case FARM_LAND_STATE.NONE:				
				PlantSeedsOnFarmLand (id);				
				break;
		/*case FARM_LAND_STATE.GROWING:				
				break;*/
			case FARM_LAND_STATE.WAITING_FOR_HARVEST:
				HarvestCropOnFarmLand (id);
				break;

			default:
				break;
		}
		//PlantSeedsOnFarmLand (id);
		//print ("Farm " + id + " MouseEnter");
	}

	public void CallParentOnMouseDrag (int id)
	{
		//print ("Farm " + id + " MouseDrag");
	}

	public void CallParentOnMouseExit (int id)
	{
		//print ("Farm " + id + " MouseExit");
	}

	public void CallParentOnMouseUpAsButton (int id)
	{
		//print ("Farm " + id + " MouseUpAsButton");
	}

	public void CallParentOnMouseDown (int id)
	{
		//print ("Farm " + id + " MouseUpAsButton");
	}

	#endregion

	void SaveFarmLands ()
	{
		foreach (var item in farmList) {
			item.dateTime = FarmLands [item.id].GetComponent <FarmLands> ().dateTime.ToString ();
			item.pos = FarmLands [item.id].GetComponent <FarmLands> ().pos;
			item.id = FarmLands [item.id].GetComponent <FarmLands> ().id;
			item.level = FarmLands [item.id].GetComponent <FarmLands> ().level;
			item.seedID = FarmLands [item.id].GetComponent <FarmLands> ().seedID;
			item.state = (sbyte)FarmLands [item.id].GetComponent <FarmLands> ().state;
		}		
		ES2.Save (farmList, "farmList");
	}
}

