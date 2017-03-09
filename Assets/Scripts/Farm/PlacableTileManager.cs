using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using TMPro;

public class PlacableTileManager : MonoBehaviour
{
	public static PlacableTileManager m_instance = null;
	public GameObject farmLandPrefab;
	public GameObject FarmLandMenu = null, FarmTimerText = null, FarmHarvestingMenu = null;

	List<SaveFarmLand> farmList = new List<SaveFarmLand> ();
	GameObject[] FarmLands;
	int farmLandCount = 0;
	System.TimeSpan remainingTime;
	int thisIndex = -1;

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
		foreach (var item in farmList) {
			InitFarmLands (item);
		}
		InvokeRepeating ("SaveFarmLands", 0, 5);
	}

	public void AddNewFarmLandInMap (int index)
	{
		print (System.DateTime.UtcNow.ToString ());
		farmList.Add (new SaveFarmLand ((sbyte)index, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
		GameObject FarmLand = Instantiate (farmLandPrefab, this.transform);
		FarmLand.transform.localPosition = new Vector3 (index, 0, 0);
		FarmLand.gameObject.name = "FarmLand" + index;
		ES2.Save (farmList, "farmList");
	}

	public void ShowFarmLandMenu (int index)
	{
		FarmLandMenu.transform.position = FarmLands [index].transform.position;
		FarmLandMenu.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		FarmLandMenu.SetActive (true);
		LeanTween.scale (FarmLandMenu, Vector3.one, 0.2f, FirstScript.m_instance.ease);		
	}

	public void ShowReadyToHarvestMenu (int index)
	{
		FarmHarvestingMenu.transform.position = FarmLands [index].transform.position;
		FarmHarvestingMenu.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		FarmHarvestingMenu.SetActive (true);
		LeanTween.scale (FarmHarvestingMenu, Vector3.one, 0.2f, FirstScript.m_instance.ease);		
	}

	public void PlantSeedsOnFarmLand (int index) // Planting Seeds
	{
		if (GameEventManager.isSeedSelected == true) {
			FarmLands [index].GetComponent <FarmLands> ().state = FARM_LAND_STATE.GROWING;
			FarmLands [index].GetComponent <FarmLands> ().dateTime = UTC.time.liveDateTime.AddMinutes (GameEventManager.seedSelectedIndex);
			FarmLands [index].GetComponent <SpriteRenderer> ().color = Color.green;
		}
	}

	public void HarvestCropOnFarmLand (int index) // Harvesting Seeds
	{
		if (GameEventManager.isScytheSelected == true) {
			FarmLands [index].GetComponent <FarmLands> ().state = FARM_LAND_STATE.NONE;
			FarmLands [index].GetComponent <FarmLands> ().dateTime = new System.DateTime ();
			FarmLands [index].GetComponent <SpriteRenderer> ().color = Color.white;
		}
	}

	public void ShowFarmLandTimeRemaining ()
	{
		FarmTimerText.transform.position = FarmLands [thisIndex].transform.position;
		remainingTime = FarmLands [thisIndex].GetComponent <FarmLands> ().dateTime.Subtract (UTC.time.liveDateTime);
		if (remainingTime <= new System.TimeSpan (360, 0, 0, 0)) { //> 1year
			FarmTimerText.transform.GetChild (0).GetComponent <TextMeshPro> ().text = remainingTime.Days.ToString () + "d " + remainingTime.Hours.ToString () + "h";
		}
		if (remainingTime <= new System.TimeSpan (1, 0, 0, 0)) { //> 1day
			FarmTimerText.transform.GetChild (0).GetComponent <TextMeshPro> ().text = remainingTime.Hours.ToString () + "h " + remainingTime.Minutes.ToString () + "min";
		}
		if (remainingTime <= new System.TimeSpan (0, 1, 0, 0)) { //> 1hr
			FarmTimerText.transform.GetChild (0).GetComponent <TextMeshPro> ().text = remainingTime.Minutes.ToString () + "m " + remainingTime.Seconds.ToString () + "sec";
		}
		if (remainingTime <= new System.TimeSpan (0, 0, 1, 0)) { // 1min
			FarmTimerText.transform.GetChild (0).GetComponent <TextMeshPro> ().text = remainingTime.Seconds.ToString () + "sec";
		}
		if (remainingTime <= new System.TimeSpan (0, 0, 0, 0)) { // 1min
			FarmTimerText.SetActive (false);
		}
	}

	void LateUpdate ()
	{
		if (FarmTimerText.activeSelf) {			
			ShowFarmLandTimeRemaining ();
		} else {
			thisIndex = -1;
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
			farmList.Add (new SaveFarmLand (0, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (1, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (2, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			ES2.Save (farmList, "farmList");
			PlayerPrefs.SetInt ("firstFarms", 1);
			print (PlayerPrefs.GetInt ("firstFarms") + " only once");
		}
	}

	public void InitFarmLands (SaveFarmLand item)
	{
		FarmLands [farmLandCount] = Instantiate (farmLandPrefab, this.transform);
		FarmLands [farmLandCount].transform.localPosition = new Vector3 (item.tileIndex, 0, 0);
		FarmLands [farmLandCount].gameObject.name = "FarmLand" + item.tileIndex;
		FarmLands [farmLandCount].GetComponent <FarmLands> ().tileIndex = item.tileIndex;
		FarmLands [farmLandCount].GetComponent <FarmLands> ().level = item.level;
		FarmLands [farmLandCount].GetComponent <FarmLands> ().seedIndex = item.seedIndex;
		FarmLands [farmLandCount].GetComponent <FarmLands> ().state = (FARM_LAND_STATE)item.state;
		switch (FarmLands [farmLandCount].GetComponent <FarmLands> ().state) {
			case FARM_LAND_STATE.NONE:
				FarmLands [farmLandCount].GetComponent<SpriteRenderer> ().color = Color.white;
				break;
			case FARM_LAND_STATE.GROWING:				
				FarmLands [farmLandCount].GetComponent<SpriteRenderer> ().color = Color.green;
				break;
			case FARM_LAND_STATE.WAITING_FOR_HARVEST:
				FarmLands [farmLandCount].GetComponent<SpriteRenderer> ().color = Color.red;
				break;
			default:
				break;
		}
		FarmLands [farmLandCount].GetComponent <FarmLands> ().dateTime = System.DateTime.Parse (item.dateTime);
		farmLandCount++;
	}

	void InitFarmLandsStates ()
	{

	}

	public void CallParentOnMouseDown (int index)
	{	
//		print ("Farm " + index + " MouseDown");
	}

	public void CallParentOnMouseUp (int index)  //ShowFarmLandMenu(int index)
	{
		switch (FarmLands [index].GetComponent <FarmLands> ().state) {
			case FARM_LAND_STATE.NONE:
				ShowFarmLandMenu (index);
				break;
			case FARM_LAND_STATE.GROWING:
				thisIndex = index;
				FarmTimerText.SetActive (true);
				break;
			case FARM_LAND_STATE.WAITING_FOR_HARVEST:
				ShowReadyToHarvestMenu (index);
				break;

			default:
				break;
		}

		//print ("Farm " + index + " MouseUp");
	}

	public void CallParentOnMouseEnter (int index)
	{
		switch (FarmLands [index].GetComponent <FarmLands> ().state) {
			case FARM_LAND_STATE.NONE:
				PlantSeedsOnFarmLand (index);
				break;
			case FARM_LAND_STATE.GROWING:
				
				break;
			case FARM_LAND_STATE.WAITING_FOR_HARVEST:
				HarvestCropOnFarmLand (index);
				break;

			default:
				break;
		}
		PlantSeedsOnFarmLand (index);
		//print ("Farm " + index + " MouseEnter");
	}

	public void CallParentOnMouseDrag (int index)
	{
		//print ("Farm " + index + " MouseDrag");
	}

	public void CallParentOnMouseExit (int index)
	{
		//print ("Farm " + index + " MouseExit");
	}

	public void CallParentOnMouseUpAsButton (int index)
	{
		//print ("Farm " + index + " MouseUpAsButton");
	}

	#endregion

	void SaveFarmLands ()
	{
		foreach (var item in farmList) {
			item.dateTime = FarmLands [item.tileIndex].GetComponent <FarmLands> ().dateTime.ToString ();
			item.tileIndex = FarmLands [item.tileIndex].GetComponent <FarmLands> ().tileIndex;
			item.level = FarmLands [item.tileIndex].GetComponent <FarmLands> ().level;
			item.seedIndex = FarmLands [item.tileIndex].GetComponent <FarmLands> ().seedIndex;
			item.state = (sbyte)FarmLands [item.tileIndex].GetComponent <FarmLands> ().state;
		}
		ES2.Save (farmList, "farmList");
		print ("gameSaved");
	}
}


