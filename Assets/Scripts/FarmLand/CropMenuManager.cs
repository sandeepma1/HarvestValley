using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CropMenuManager : MonoBehaviour
{
	public static CropMenuManager m_instance = null;
	public GameObject seedPrefab;
	public List<GameObject> seeds;
	//public GameObject[] menuPages;
	public GameObject switchButton, fieldInfoButton, seedMenuBackground;
	public bool isSeedSelected = false;
	public int seedSelectedID = -1;

	Vector2[] pos = new Vector2[4];
	//number of seeds in menu => 4
	int a = 0;
	int maxSeedsOnceInMenu = 4;
	int maxPages = 0;
	List<int> unlockedSeedIDs = new List<int> ();

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		pos [0] = new Vector2 (-0.5f, -0.6f);
		pos [1] = new Vector2 (0.5f, -0.6f);
		pos [2] = new Vector2 (-0.5f, 0.25f);
		pos [3] = new Vector2 (0.5f, 0.25f);
		CheckForUnlockedSeeds ();
	}

	void Update ()
	{
		/*DebugTextHandler.m_instance.DisplayDebugText ("seeds " + seeds.Count + " unlockedSeedID " + unlockedSeedIDs.Count +
		" playerInv " + PlayerInventoryManager.m_instance.playerInventory.Count);*/
	}

	public void CheckForUnlockedSeeds ()  // call on level change & game start only
	{
		unlockedSeedIDs.Clear ();
		for (int i = 0; i <= PlayerProfileManager.m_instance.CurrentPlayerLevel (); i++) {
			if (LevelUpDatabase.m_instance.gameLevels [i].itemUnlockID >= 0) {
				unlockedSeedIDs.Add (LevelUpDatabase.m_instance.gameLevels [i].itemUnlockID);
			}		
		}
		if (unlockedSeedIDs.Count > 4) {
			switchButton.SetActive (true);
		}
		maxPages = unlockedSeedIDs.Count / maxSeedsOnceInMenu;
		if (unlockedSeedIDs.Count % maxSeedsOnceInMenu >= 1) {
			maxPages++;
		}
		PopulateSeedsInSeedMenu ();
	}

	public void PopulateSeedsInSeedMenu ()
	{
		if (seeds != null) {
			foreach (var seed in seeds) {
				Destroy (seed);
			}
		}
		//seeds = new GameObject[unlockedSeedIDs.Count];
		seeds.Clear ();
		int posCount = 0;
		for (int i = 0; i < unlockedSeedIDs.Count; i++) {
			seeds.Add (Instantiate (seedPrefab, this.gameObject.transform.GetChild (0).transform));
			//seeds [i] = Instantiate (seedPrefab, this.gameObject.transform.GetChild (0).transform);
			seeds [i].name = "Seed" + i;
			seeds [i].GetComponent <DraggableSeeds> ().seedID = unlockedSeedIDs [i];
			seeds [i].transform.GetChild (0).GetComponent <SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Textures/Crop/" + ItemDatabase.m_instance.items [unlockedSeedIDs [i]].name);
			seeds [i].transform.GetChild (1).GetComponent<TextMeshPro> ().text = PlayerInventoryManager.m_instance.playerInventory [i].count.ToString ();
			seeds [i].transform.localPosition = pos [posCount];
			posCount++;
			if (posCount > pos.Length - 1) {
				posCount = 0;
			}
			seeds [i].SetActive (false);
		}
		ToggleMenuPages ();
	}

	public void UpdateSeedValue ()
	{
		for (int i = 0; i < unlockedSeedIDs.Count; i++) {
			seeds [i].transform.GetChild (1).GetComponent<TextMeshPro> ().text = PlayerInventoryManager.m_instance.playerInventory [i].count.ToString ();
		}
		PlayerInventoryManager.m_instance.UpdateScrollListItemCount ();
	}

	public void ToggleMenuPages ()
	{
		foreach (var seed in seeds) {
			seed.SetActive (false);
		}
		int loopCount = 0;

		for (int i = a * maxSeedsOnceInMenu; i < unlockedSeedIDs.Count; i++) {			
			seeds [i].SetActive (true);
			loopCount++;
			if (loopCount >= 4) {
				break;
			}
		}
		a++;
		if (a >= maxPages) {
			a = 0;
		}
	}

	public void ChildCallingOnMouseUp (int id)
	{
		isSeedSelected = false;
		PlacableTileManager.m_instance.plantedOnSelectedfield = false;
		seedSelectedID = -1;
	}

	public void ChildCallingOnMouseDrag (int id)
	{
		isSeedSelected = true;
		seedSelectedID = id;	
		ToggleDisplayCropMenu ();
	}

	public void ToggleDisplayCropMenu ()
	{		
		transform.position = new Vector3 (-500, -500, 0);
	}
}
