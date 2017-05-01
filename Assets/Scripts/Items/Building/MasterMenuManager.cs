using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MasterMenuManager : MonoBehaviour
{
	public static MasterMenuManager m_instance = null;
	public GameObject menuItemPrefab;
	public GameObject[] menuBackground = new GameObject[3];
	public GameObject[] menuItems = new GameObject[12];
	public GameObject switchButton, fieldInfoButton;
	public bool isItemSelected = false;
	public int itemSelectedID = -1;
	public bool isMasterMenuUp = false;
	Vector2[] itemPos = new Vector2[4];
	int pageNumber = 0;
	int maxItemsOnceInMenu = 4;
	int maxPages = 0;
	int unlockedItemCount = 0;
	List<int> unlockedItemIDs = new List<int> ();

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		itemPos [0] = new Vector2 (-0.5f, -0.6f);
		itemPos [1] = new Vector2 (0.5f, -0.6f);
		itemPos [2] = new Vector2 (-0.5f, 0.25f);
		itemPos [3] = new Vector2 (0.5f, 0.25f);	
		SpwanMenuItems ();
		CheckForUnlockedItems ();
	}

	void SpwanMenuItems ()
	{
		for (int i = 0; i < menuItems.Length; i++) {
			menuItems [i] = (Instantiate (menuItemPrefab, this.gameObject.transform.GetChild (0).transform));
			menuItems [i].name = "Item" + i;
		}
	}

	public void CheckForUnlockedItems ()  // call on level change & game start only
	{
		unlockedItemIDs.Clear ();
		for (int i = 0; i <= PlayerProfileManager.m_instance.CurrentPlayerLevel (); i++) {
			if (LevelUpDatabase.m_instance.gameLevels [i].itemUnlockID >= 0) {
				unlockedItemIDs.Add (LevelUpDatabase.m_instance.gameLevels [i].itemUnlockID);
			}
		}

		if (unlockedItemIDs.Count > 4) {
			switchButton.SetActive (true);
		}
		maxPages = unlockedItemIDs.Count / maxItemsOnceInMenu;
		if (unlockedItemIDs.Count % maxItemsOnceInMenu >= 1) {
			maxPages++;
		}
		foreach (var item in unlockedItemIDs) {
			print ("unlocked items " + item);
		}
	}

	//PopulateItemsInMasterMenu
	public void PopulateItemsInMasterMenu (int buildingID)
	{
		foreach (var item in menuItems) {
			item.SetActive (false);
			item.GetComponent <DraggableItems> ().itemID = -1;
		}
		int posCount = 0;
		unlockedItemCount = 0;	
		pageNumber = 0;
		isMasterMenuUp = true;
		/*foreach (var item in ItemDatabase.m_instance.items) {
			if (item != null && item.source == BuildingDatabase.m_instance.buildingInfo [buildingID].name) {
				menuItems [unlockedItemCount].GetComponent <DraggableItems> ().itemID = item.id;
				menuItems [unlockedItemCount].transform.localPosition = itemPos [posCount];
				menuItems [unlockedItemCount].transform.GetChild (0).GetComponent <SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Textures/Items/" + item.name);
				menuItems [posCount].transform.GetChild (1).GetComponent<TextMeshPro> ().text = PlayerInventoryManager.m_instance.playerInventory [buildingID].count.ToString ();
				posCount++;
				unlockedItemCount++;
				if (posCount > itemPos.Length - 1) {
					posCount = 0;
				}
				//menuItems [posCount].SetActive (false);
			}
		}*/
		for (int i = 0; i < unlockedItemIDs.Count; i++) {
			if (ItemDatabase.m_instance.items [i] != null && ItemDatabase.m_instance.items [i].source == BuildingDatabase.m_instance.buildingInfo [buildingID].name) {
				menuItems [unlockedItemCount].GetComponent <DraggableItems> ().itemID = ItemDatabase.m_instance.items [i].id;
				menuItems [unlockedItemCount].transform.localPosition = itemPos [posCount];
				menuItems [unlockedItemCount].transform.GetChild (0).GetComponent <SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Textures/Items/" + ItemDatabase.m_instance.items [i].name);
				menuItems [posCount].transform.GetChild (1).GetComponent<TextMeshPro> ().text = PlayerInventoryManager.m_instance.playerInventory [i].count.ToString ();
				posCount++;
				unlockedItemCount++;
				if (posCount > itemPos.Length - 1) {
					posCount = 0;
				}
				//menuItems [posCount].SetActive (false);
			}
		}
		maxPages = unlockedItemCount / maxItemsOnceInMenu;
		if (unlockedItemCount % maxItemsOnceInMenu >= 1) { //Calculate Max pages
			maxPages++;
		}
		ToggleMenuPages ();
	}

	public void UpdateSeedValue ()
	{
		for (int i = 0; i < unlockedItemCount; i++) {
			menuItems [i].transform.GetChild (1).GetComponent<TextMeshPro> ().text = PlayerInventoryManager.m_instance.playerInventory [i].count.ToString ();
		}
		PlayerInventoryManager.m_instance.UpdateScrollListItemCount ();
	}

	public void ToggleMenuPages ()
	{
		foreach (var items in menuItems) {
			items.SetActive (false);
		}
		int loopCount = 0;
		for (int i = pageNumber * maxItemsOnceInMenu; i < unlockedItemCount; i++) {			
			menuItems [i].SetActive (true);
			loopCount++;
			if (loopCount >= 4) {
				break;
			}
		}
		pageNumber++;
		if (pageNumber >= maxPages) {
			pageNumber = 0;
		}
	}

	public void ChildCallingOnMouseUp (int id)
	{
		isItemSelected = false;
		BuildingsManager.m_instance.plantedOnSelectedfield = false;
		itemSelectedID = -1;
	}

	public void ChildCallingOnMouseDrag (int id)
	{
		isItemSelected = true;
		itemSelectedID = id;
		//if (id == 0) { // if feild building is selected only then hide the master menu
		ToggleDisplayCropMenu ();
		//}
	}

	public void ToggleDisplayCropMenu ()
	{
		transform.position = new Vector3 (-500, -500, 0);
	}
}
