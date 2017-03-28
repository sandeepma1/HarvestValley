using System.Collections;
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
	public List<FarmField> farmList = new List<FarmField> ();
	GameObject[] FarmLands;
	System.TimeSpan remainingTime;
	int tempID = -1, longPressFarmID = -1, mouseDownFarmID = -1;
	bool isLongPress = false;
	bool isTilePressed = false;
	float longPressTime = 0.5f, longPressTimer = 0f;

	public bool plantedOnSelectedfield = false;
	public int fieldSelectedID = -1;

	void Awake ()
	{
		m_instance = this;
		OneTimeOnly ();
		Init ();
	}

	void Init ()
	{
		farmList = ES2.LoadList<FarmField> ("FarmField");
		FarmLands = new GameObject[farmList.Count];
		foreach (var farm in farmList) {
			InitFarmLands (farm);
		}
		InvokeRepeating ("SaveFarmLands", 0, 5);
	}

	public void AddNewFarmLandInMap (int farmLandID)
	{
		print (System.DateTime.UtcNow.ToString ());
		//farmList.Add (new SaveFarmLand (id, 1, 0, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
		GameObject FarmLand = Instantiate (farmLandPrefab, this.transform);
		FarmLand.transform.localPosition = new Vector3 (farmLandID, 0, 0);
		FarmLand.gameObject.name = "FarmField" + farmLandID;
		ES2.Save (farmList, "FarmField");
	}

	public void ShowFarmLandMenu (int farmLandID) // Display field Crop Menu
	{
		IGMMenu.m_instance.DisableAllMenus ();
		fieldSelectedID = farmLandID;
		CropMenu.transform.position = FarmLands [farmLandID].transform.position;
		CropMenu.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		CropMenu.SetActive (true);
		LeanTween.scale (CropMenu, Vector3.one, 0.2f, IGMMenu.m_instance.ease);
	}

	public void ShowReadyToHarvestMenu (int farmLandID) // Display Harvesting Menu
	{
		IGMMenu.m_instance.DisableAllMenus ();
		FarmHarvestingMenu.transform.position = FarmLands [farmLandID].transform.position;
		FarmHarvestingMenu.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		FarmHarvestingMenu.SetActive (true);
		LeanTween.scale (FarmHarvestingMenu, Vector3.one, 0.2f, IGMMenu.m_instance.ease);		
	}

	public void PlantSeedsOnFarmLand (int farmLandID) // Planting Seeds
	{		
		if (CropMenuManager.m_instance.isSeedSelected == true) {	
			//if (fieldSelectedID == farmLandID || plantedOnSelectedfield && PlayerInventoryManager.m_instance.playerInventory [CropMenuManager.m_instance.seedSelectedID].count >= 1) {							
			if (plantedOnSelectedfield || fieldSelectedID == farmLandID) {				
				if (PlayerInventoryManager.m_instance.playerInventory [CropMenuManager.m_instance.seedSelectedID].count >= 1) {
					FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().state = FARM_LAND_STATE.GROWING;
					FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().seedID = CropMenuManager.m_instance.seedSelectedID;
					FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().dateTime = UTC.time.liveDateTime.AddMinutes (ItemDatabase.m_instance.items [CropMenuManager.m_instance.seedSelectedID].timeRequiredInMins);
					FarmLands [farmLandID].GetComponent <SpriteRenderer> ().color = Color.green;
					PlayerInventoryManager.m_instance.playerInventory [CropMenuManager.m_instance.seedSelectedID].count--;
					CropMenuManager.m_instance.UpdateSeedValue ();
					SaveFarmLands ();
					plantedOnSelectedfield = true;
					fieldSelectedID = -1;
					//CropMenuManager.m_instance.ToggleDisplayCropMenu ();	
				}				
			}
		}
	}

	public void HarvestCropOnFarmLand (int farmLandID) // Harvesting Seeds
	{
		if (HarvestMenuManager.m_instance.isScytheSelected == true) {
			// TODO Heavy update required for field Level Based cals*******************
			// only 2 items are added in storage
//			print (FarmLands [farmLandID].GetComponent <FarmLands> ().seedID);
			PlayerInventoryManager.m_instance.UpdateFarmItems (FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().seedID, 2);
			PlayerProfileManager.m_instance.PlayerXPPointsAdd (ItemDatabase.m_instance.items [FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().seedID].XP);
			FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().state = FARM_LAND_STATE.NONE;
			FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().dateTime = new System.DateTime ();
			FarmLands [farmLandID].GetComponent <SpriteRenderer> ().color = Color.white;
			HarvestMenuManager.m_instance.ToggleDisplayHarvestingMenu ();
		}
	}

	public void DisableAnyOpenMenus ()
	{
		for (int i = 0; i < FarmLands.Length; i++) {
			FarmLands [i].GetComponent <DraggableFarmLands> ().isSelected = false;
		}
		isLongPress = false;
	}

	public void ShowFarmLandTimeRemaining ()
	{
		FarmTimerText.transform.position = FarmLands [tempID].transform.position;
		remainingTime = FarmLands [tempID].GetComponent <DraggableFarmLands> ().dateTime.Subtract (UTC.time.liveDateTime);
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
			if (item.GetComponent <DraggableFarmLands> ().state == FARM_LAND_STATE.GROWING && item.GetComponent <DraggableFarmLands> ().dateTime.Subtract (UTC.time.liveDateTime) <= new System.TimeSpan (0, 0, 0)) {				
				item.GetComponent <DraggableFarmLands> ().state = FARM_LAND_STATE.WAITING_FOR_HARVEST;
				item.GetComponent <DraggableFarmLands> ().dateTime = new System.DateTime ();
				item.GetComponent <SpriteRenderer> ().color = Color.red;
			}
		}
	}

	void Update ()
	{
		if (isTilePressed) {			
			if (longPressTimer >= longPressTime) {				
				isLongPress = true;
				longPressFarmID = mouseDownFarmID;
				mouseDownFarmID = -1;
				isTilePressed = false;
				FarmLands [longPressFarmID].GetComponent <DraggableFarmLands> ().isSelected = true;
				return;
			}
			longPressTimer += Time.deltaTime;
		}
	}

	#region OnMouse Functions

	void OneTimeOnly ()
	{ 
		if (PlayerPrefs.GetInt ("firstFarms") <= 0) {			
			ES2.Delete ("FarmField");
			farmList.Add (new FarmField (0, new Vector2 (0, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new FarmField (1, new Vector2 (1, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new FarmField (2, new Vector2 (2, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new FarmField (3, new Vector2 (3, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new FarmField (4, new Vector2 (4, 0), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new FarmField (5, new Vector2 (0, -1), 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			ES2.Save (farmList, "FarmField");
			PlayerPrefs.SetInt ("firstFarms", 1);
		}
	}

	public void InitFarmLands (FarmField farm)
	{
		FarmLands [farm.id] = Instantiate (farmLandPrefab, this.transform);
		FarmLands [farm.id].transform.localPosition = farm.pos;
		FarmLands [farm.id].gameObject.name = "FarmLand" + farm.id;
		FarmLands [farm.id].GetComponent <DraggableFarmLands> ().id = farm.id;
		FarmLands [farm.id].GetComponent <DraggableFarmLands> ().pos = farm.pos;
		FarmLands [farm.id].GetComponent <DraggableFarmLands> ().level = farm.level;
		FarmLands [farm.id].GetComponent <DraggableFarmLands> ().seedID = farm.seedID;
		FarmLands [farm.id].GetComponent <DraggableFarmLands> ().state = (FARM_LAND_STATE)farm.state;
		switch (FarmLands [farm.id].GetComponent <DraggableFarmLands> ().state) {
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
		FarmLands [farm.id].GetComponent <DraggableFarmLands> ().dateTime = System.DateTime.Parse (farm.dateTime);
	}

	public void CallParentOnMouseDown (int farmLandID)
	{		
		isTilePressed = true;
		longPressTimer = 0;
		mouseDownFarmID = farmLandID;
	}

	public void CallParentOnMouseUp (int farmLandID)
	{
		print (farmLandID + " " + longPressFarmID);
		isTilePressed = false;
		mouseDownFarmID = -1;
		if (!isLongPress) {			
			switch (FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().state) {
				case FARM_LAND_STATE.NONE:
					CropMenuManager.m_instance.UpdateSeedValue ();
					ShowFarmLandMenu (farmLandID);
					break;
				case FARM_LAND_STATE.GROWING:
					tempID = farmLandID;
					IGMMenu.m_instance.DisableAllMenus ();
					FarmTimerText.SetActive (true);
					isFarmTimerEnabled = true;
					FarmTimerText.transform.GetChild (0).GetComponent <TextMeshPro> ().text = ItemDatabase.m_instance.items [FarmLands [tempID].GetComponent <DraggableFarmLands> ().seedID].name.ToString ();
					break;
				case FARM_LAND_STATE.WAITING_FOR_HARVEST:
					ShowReadyToHarvestMenu (farmLandID);
					break;
				default:
					break;
			}
		} else {
			if (farmLandID != longPressFarmID) {
				FarmLands [longPressFarmID].GetComponent <DraggableFarmLands> ().isSelected = false;
				isLongPress = false;
			}
		}
	}

	public void CallParentOnMouseEnter (int farmLandID)
	{
		switch (FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().state) {
			case FARM_LAND_STATE.NONE:				
				PlantSeedsOnFarmLand (farmLandID);				
				break;
			case FARM_LAND_STATE.WAITING_FOR_HARVEST:
				HarvestCropOnFarmLand (farmLandID);
				break;

			default:
				break;
		}
	}

	public void CallParentOnMouseDrag (int farmLandID)
	{
		if (FarmLands [farmLandID].GetComponent <DraggableFarmLands> ().isSelected) {		
			FarmLands [farmLandID].transform.position = new Vector3 (Mathf.Round (Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, 0, 0)).x),
				Mathf.Round (Camera.main.ScreenToWorldPoint (new Vector3 (0, Input.mousePosition.y, 0)).y), 0);
		}
	}

	public void CallParentOnMouseExit (int farmLandID)
	{
		//print ("Farm " + id + " MouseExit");
	}

	public void CallParentOnMouseUpAsButton (int farmLandID)
	{
		//print ("Farm " + id + " MouseUpAsButton");
	}

	#endregion

	void SaveFarmLands ()
	{
		foreach (var item in farmList) {
			item.dateTime = FarmLands [item.id].GetComponent <DraggableFarmLands> ().dateTime.ToString ();
			//item.pos = FarmLands [item.id].GetComponent <DraggableFarmLands> ().pos;
			item.pos = FarmLands [item.id].transform.localPosition;
			item.id = FarmLands [item.id].GetComponent <DraggableFarmLands> ().id;
			item.level = FarmLands [item.id].GetComponent <DraggableFarmLands> ().level;
			item.seedID = FarmLands [item.id].GetComponent <DraggableFarmLands> ().seedID;
			item.state = (sbyte)FarmLands [item.id].GetComponent <DraggableFarmLands> ().state;
		}		
		ES2.Save (farmList, "FarmField");
	}
}


[System.Serializable]
public class FarmField  // iLIST
{
	public int id;
	public Vector2 pos;
	public int level;
	public int seedID;
	public int state;
	public string dateTime;

	public FarmField ()
	{		
		id = -1;
		pos = new Vector2 (0, 0);
		level = -1;
		seedID = -1;
		state = -1;
		dateTime = "";
	}

	public FarmField (int f_id, Vector2 f_pos, int f_level, int f_seedID, int f_state, string f_dateTime)
	{		
		id = f_id;
		pos = f_pos;
		level = f_level;
		seedID = f_seedID;
		state = f_state;
		dateTime = f_dateTime;
	}
}

public enum FARM_LAND_STATE
{
	NONE,
	GROWING,
	WAITING_FOR_HARVEST}

;