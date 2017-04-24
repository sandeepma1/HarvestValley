using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using TMPro;
using UnityEngine.EventSystems;

public class BuildingsManager : MonoBehaviour
{
	public static BuildingsManager m_instance = null;
	public GameObject buildingPrefab;
	public GameObject MasterMenuGO = null, FarmTimerText = null, FarmHarvestingMenu = null, ItemMenu;
	public bool isFarmTimerEnabled = false;
	public List<Buildings> buildings = new List<Buildings> ();
	public bool plantedOnSelectedfield = false;
	public int buildingSelectedID = -1;

	GameObject[] BuildingsGO;
	System.TimeSpan remainingTime;
	int tempID = -1, longPressBuildingID = -1, mouseDownBuildingID = -1;
	bool isLongPress = false;
	bool isTilePressed = false;
	float longPressTime = 0.5f, longPressTimer = 0f;

	void Awake ()
	{
		m_instance = this;
		OneTimeOnly ();
		Init ();
	}

	void Init ()
	{
		buildings = ES2.LoadList<Buildings> ("AllBuildings");
		BuildingsGO = new GameObject[buildings.Count];
		foreach (var building in buildings) {
			InitBuildings (building);
		}
		InvokeRepeating ("SaveBuildings", 0, 5);
	}

	public void AddNewFarmLandInMap (int buildingID)
	{
		/*print (System.DateTime.UtcNow.ToString ());
		//farmList.Add (new SaveFarmLand (id, 1, 0, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
		GameObject FarmLand = Instantiate (farmLandPrefab, this.transform);
		FarmLand.transform.localPosition = new Vector3 (buildingID, 0, 0);
		FarmLand.gameObject.name = "Building" + buildingID;
		ES2.Save (buildings, "AllBuildings");*/
	}

	public void DisplayMasterMenuOnClick (int buildingID) // Display field Crop Menu
	{
		MasterMenuManager.m_instance.PopulateItemsInMasterMenu (buildingID);
		IGMMenu.m_instance.DisableAllMenus ();
		buildingSelectedID = buildingID;
		MasterMenuGO.transform.position = BuildingsGO [buildingID].transform.position;
		MasterMenuGO.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		MasterMenuGO.SetActive (true);
		LeanTween.scale (MasterMenuGO, Vector3.one, 0.2f, IGMMenu.m_instance.ease);
	}

	public void ShowReadyToHarvestMenu (int buildingID) // Display Harvesting Menu
	{
		IGMMenu.m_instance.DisableAllMenus ();
		FarmHarvestingMenu.transform.position = BuildingsGO [buildingID].transform.position;
		FarmHarvestingMenu.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		FarmHarvestingMenu.SetActive (true);
		LeanTween.scale (FarmHarvestingMenu, Vector3.one, 0.2f, IGMMenu.m_instance.ease);		
	}

	public void PlantItemsOnBuildings (int buildingID) // Planting Items
	{
		print (buildingID);
		if (MasterMenuManager.m_instance.isItemSelected == true) {
			//if (fieldSelectedID == buildingID || plantedOnSelectedfield && PlayerInventoryManager.m_instance.playerInventory [CropMenuManager.m_instance.seedSelectedID].count >= 1) {							
			if (true) {
				
			}
			if (plantedOnSelectedfield || buildingSelectedID == buildingID) {

				//	if (PlayerInventoryManager.m_instance.playerInventory [CropMenuManager.m_instance.seedSelectedID].count >= 1) {
				BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().state = BUILDINGS_STATE.GROWING;
				BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().itemID1 = MasterMenuManager.m_instance.itemSelectedID;
				BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().dateTime1 = UTC.time.liveDateTime.AddMinutes (ItemDatabase.m_instance.items [MasterMenuManager.m_instance.itemSelectedID].timeRequiredInMins);
				BuildingsGO [buildingID].GetComponent <SpriteRenderer> ().color = Color.green;
				PlayerInventoryManager.m_instance.playerInventory [MasterMenuManager.m_instance.itemSelectedID].count--;
				MasterMenuManager.m_instance.UpdateSeedValue ();
				SaveBuildings ();
				plantedOnSelectedfield = true;
				buildingSelectedID = -1;
				//}				
			}
		}
	}

	public void HarvestCropOnFarmLand (int buildingID) // Harvesting Seeds
	{
		if (HarvestMenuManager.m_instance.isScytheSelected == true) {
			// TODO Heavy update required for field Level Based cals*******************
			// only 2 items are added in storage
//			print (FarmLands [buildingID].GetComponent <FarmLands> ().itemID);
			PlayerInventoryManager.m_instance.UpdateFarmItems (BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().itemID1, 2);
			PlayerProfileManager.m_instance.PlayerXPPointsAdd (ItemDatabase.m_instance.items [BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().itemID1].XP);
			BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().state = BUILDINGS_STATE.NONE;
			BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().dateTime1 = new System.DateTime ();
			BuildingsGO [buildingID].GetComponent <SpriteRenderer> ().color = Color.white;
			HarvestMenuManager.m_instance.ToggleDisplayHarvestingMenu ();
		}
	}

	public void DisableAnyOpenMenus ()
	{
		for (int i = 0; i < BuildingsGO.Length; i++) {
			BuildingsGO [i].GetComponent <DraggableBuildings> ().isSelected = false;
			DisableOutlineOnSprite (i);
		}
		isLongPress = false;
	}

	public void ShowFarmLandTimeRemaining ()
	{
		FarmTimerText.transform.position = BuildingsGO [tempID].transform.position;
		remainingTime = BuildingsGO [tempID].GetComponent <DraggableBuildings> ().dateTime1.Subtract (UTC.time.liveDateTime);
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

	void LateUpdate () //Mainly used to show time remaining
	{
		if (isFarmTimerEnabled) {			
			ShowFarmLandTimeRemaining ();
		} else {
			tempID = -1;
		}
		foreach (var item in BuildingsGO) {  //Main loop for checking all buildings time
			if (item.GetComponent <DraggableBuildings> ().state == BUILDINGS_STATE.GROWING && item.GetComponent <DraggableBuildings> ().dateTime1.Subtract (UTC.time.liveDateTime) <= new System.TimeSpan (0, 0, 0)) {				
				item.GetComponent <DraggableBuildings> ().state = BUILDINGS_STATE.WAITING_FOR_HARVEST;
				item.GetComponent <DraggableBuildings> ().dateTime1 = new System.DateTime ();
				item.GetComponent <SpriteRenderer> ().color = Color.red;
			}
		}
	}

	void Update () // all long press logic	
	{
		if (isTilePressed) {	// all long press logic		
			if (longPressTimer >= longPressTime) {				
				isLongPress = true;
				longPressBuildingID = mouseDownBuildingID;
				mouseDownBuildingID = -1;
				isTilePressed = false;
				BuildingsGO [longPressBuildingID].GetComponent <DraggableBuildings> ().isSelected = true;
				EnableOutlineOnSprite (longPressBuildingID);
				return;
			}
			longPressTimer += Time.deltaTime;
		}
	}

	void EnableOutlineOnSprite (int selectedFieldID)
	{
		BuildingsGO [selectedFieldID].GetComponent <Renderer> ().material.color = new Color (1, 1, 1, 1);
	}

	void DisableOutlineOnSprite (int selectedFieldID)
	{
		BuildingsGO [selectedFieldID].GetComponent <Renderer> ().material.color = new Color (1, 1, 1, 0);
	}

	#region OnMouse Functions

	void OneTimeOnly ()
	{ 
		if (PlayerPrefs.GetInt ("firstBuilding") <= 0) {			
			ES2.Delete ("AllBuildings");
			buildings.Add (new Buildings (0, "Field", new Vector2 (0, 0), 1, 0, 0, System.DateTime.UtcNow.ToString (), 0, System.DateTime.UtcNow.ToString (), 0, System.DateTime.UtcNow.ToString ()));
			buildings.Add (new Buildings (1, "Bakery", new Vector2 (1, 0), 1, 0, 0, System.DateTime.UtcNow.ToString (), 0, System.DateTime.UtcNow.ToString (), 0, System.DateTime.UtcNow.ToString ()));
			buildings.Add (new Buildings (2, "FeedMill", new Vector2 (2, 0), 1, 0, 0, System.DateTime.UtcNow.ToString (), 0, System.DateTime.UtcNow.ToString (), 0, System.DateTime.UtcNow.ToString ()));
			buildings.Add (new Buildings (3, "Dairy", new Vector2 (3, 0), 1, 0, 0, System.DateTime.UtcNow.ToString (), 0, System.DateTime.UtcNow.ToString (), 0, System.DateTime.UtcNow.ToString ()));
			ES2.Save (buildings, "AllBuildings");
			PlayerPrefs.SetInt ("firstBuilding", 1);
		}
	}

	public void InitBuildings (Buildings building)
	{
		BuildingsGO [building.id] = Instantiate (buildingPrefab, this.transform);
		BuildingsGO [building.id].transform.localPosition = building.pos;
		BuildingsGO [building.id].gameObject.name = "Building" + building.id;
		BuildingsGO [building.id].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Textures/Buildings/" + building.name);
		BuildingsGO [building.id].GetComponent <DraggableBuildings> ().id = building.id;
		BuildingsGO [building.id].GetComponent <DraggableBuildings> ().pos = building.pos;
		BuildingsGO [building.id].GetComponent <DraggableBuildings> ().level = building.level;
		BuildingsGO [building.id].GetComponent <DraggableBuildings> ().itemID1 = building.itemID1;
		BuildingsGO [building.id].GetComponent <DraggableBuildings> ().state = (BUILDINGS_STATE)building.state;
		DisableOutlineOnSprite (building.id);
		switch (BuildingsGO [building.id].GetComponent <DraggableBuildings> ().state) {
			case BUILDINGS_STATE.NONE:
				BuildingsGO [building.id].GetComponent<SpriteRenderer> ().color = Color.white;
				break;
			case BUILDINGS_STATE.GROWING:				
				BuildingsGO [building.id].GetComponent<SpriteRenderer> ().color = Color.green;
				break;
			case BUILDINGS_STATE.WAITING_FOR_HARVEST:
				BuildingsGO [building.id].GetComponent<SpriteRenderer> ().color = Color.red;
				break;
			default:
				break;
		}
		BuildingsGO [building.id].GetComponent <DraggableBuildings> ().dateTime1 = System.DateTime.Parse (building.dateTime1);
	}

	public void CallParentOnMouseDown (int buildingID)
	{		
		isTilePressed = true;
		longPressTimer = 0;
		mouseDownBuildingID = buildingID;
		if (buildingID != longPressBuildingID && longPressBuildingID != -1) {
			BuildingsGO [longPressBuildingID].GetComponent <DraggableBuildings> ().isSelected = false;
			DisableOutlineOnSprite (longPressBuildingID);
			isLongPress = false;
		}
	}

	public void CallParentOnMouseUp (int buildingID)
	{		
		isTilePressed = false;
		mouseDownBuildingID = -1;
		if (!isLongPress) {			
			switch (BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().state) {
				case BUILDINGS_STATE.NONE:
					CropMenuManager.m_instance.UpdateSeedValue ();
					DisplayMasterMenuOnClick (buildingID);
					break;
				case BUILDINGS_STATE.GROWING:
					tempID = buildingID;
					IGMMenu.m_instance.DisableAllMenus ();
					FarmTimerText.SetActive (true);
					isFarmTimerEnabled = true;
					FarmTimerText.transform.GetChild (0).GetComponent <TextMeshPro> ().text = ItemDatabase.m_instance.items [BuildingsGO [tempID].GetComponent <DraggableBuildings> ().itemID1].name.ToString ();
					break;
				case BUILDINGS_STATE.WAITING_FOR_HARVEST:
					ShowReadyToHarvestMenu (buildingID);
					break;
				default:
					break;
			}
		} else {
			if (buildingID != longPressBuildingID) {
				BuildingsGO [longPressBuildingID].GetComponent <DraggableBuildings> ().isSelected = false;
				DisableOutlineOnSprite (longPressBuildingID);
				isLongPress = false;
			}
		}
	}

	public void CallParentOnMouseEnter (int buildingID)
	{
		switch (BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().state) {
			case BUILDINGS_STATE.NONE:				
				PlantItemsOnBuildings (buildingID);				
				break;
			case BUILDINGS_STATE.WAITING_FOR_HARVEST:
				HarvestCropOnFarmLand (buildingID);
				break;
			default:
				break;
		}
	}

	public void CallParentOnMouseDrag (int buildingID)
	{
		if (BuildingsGO [buildingID].GetComponent <DraggableBuildings> ().isSelected) {		
			BuildingsGO [buildingID].transform.position = new Vector3 (Mathf.Round (Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, 0, 0)).x),
				Mathf.Round (Camera.main.ScreenToWorldPoint (new Vector3 (0, Input.mousePosition.y, 0)).y), 0);
		}
	}

	#endregion

	void SaveBuildings ()
	{
		foreach (var item in buildings) {
			item.dateTime1 = BuildingsGO [item.id].GetComponent <DraggableBuildings> ().dateTime1.ToString ();
			//item.pos = FarmLands [item.id].GetComponent <DraggableBuildings> ().pos;
			item.pos = BuildingsGO [item.id].transform.localPosition;
			item.id = BuildingsGO [item.id].GetComponent <DraggableBuildings> ().id;
			item.level = BuildingsGO [item.id].GetComponent <DraggableBuildings> ().level;
			item.itemID1 = BuildingsGO [item.id].GetComponent <DraggableBuildings> ().itemID1;
			item.state = (sbyte)BuildingsGO [item.id].GetComponent <DraggableBuildings> ().state;
		}		
		ES2.Save (buildings, "AllBuildings");
	}
}

[System.Serializable]
public class Buildings  // iLIST
{
	public int id;
	public string name;
	public Vector2 pos;
	public int level;
	public int state;
	public int itemID1;
	public string dateTime1;
	public int itemID2;
	public string dateTime2;
	public int itemID3;
	public string dateTime3;

	public Buildings ()
	{				
	}

	public Buildings (int f_id, string f_name, Vector2 f_pos, int f_level, int f_state, int f_itemID1, string f_dateTime1, int f_itemID2, string f_dateTime2, int f_itemID3, string f_dateTime3)
	{		
		id = f_id;
		name = f_name;
		pos = f_pos;
		level = f_level;
		state = f_state;
		itemID1 = f_itemID1;
		dateTime1 = f_dateTime1;
		itemID2 = f_itemID2;
		dateTime2 = f_dateTime2;
		itemID3 = f_itemID3;
		dateTime3 = f_dateTime3;
	}
}

public enum BUILDINGS_STATE
{
	NONE,
	GROWING,
	WAITING_FOR_HARVEST}

;