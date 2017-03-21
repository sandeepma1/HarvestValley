using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CropMenuManager : MonoBehaviour
{
	public static CropMenuManager m_instance = null;
	public GameObject seedPrefab;
	public GameObject[] seeds;
	//public GameObject[] menuPages;
	public GameObject switchButton, fieldInfoButton;
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

	public void CheckForUnlockedSeeds ()  // call on level change
	{
		unlockedSeedIDs.Clear ();
		foreach (var item in ItemDatabase.m_instance.items) {				
			if (item.unlocksAtLevel <= PlayerProfileManager.m_instance.CurrentPlayerLevel ()) {					
				unlockedSeedIDs.Add (item.id);
			}
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
		seeds = new GameObject[unlockedSeedIDs.Count];
		int posCount = 0;
		for (int i = 0; i < unlockedSeedIDs.Count; i++) {
			seeds [i] = Instantiate (seedPrefab, this.gameObject.transform.GetChild (0).transform);
			seeds [i].name = "Seed" + i;
			seeds [i].GetComponent <DraggableSeeds> ().seedID = unlockedSeedIDs [i];
			seeds [i].transform.GetChild (0).GetComponent <SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Textures/Crop/" + ItemDatabase.m_instance.items [unlockedSeedIDs [i]].name);
			seeds [i].transform.GetChild (1).GetComponent<TextMeshPro> ().text = FarmItemsManager.m_instance.playerItems [i].count.ToString ();
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
			seeds [i].transform.GetChild (1).GetComponent<TextMeshPro> ().text = FarmItemsManager.m_instance.playerItems [i].count.ToString ();
		}
		FarmItemsManager.m_instance.UpdateScrollListItemCount ();
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
		if (unlockedSeedIDs.Count >= 5) { // show switch page button only if seeds unlocked are more than 4
			switchButton.SetActive (true);	
		}
		fieldInfoButton.SetActive (true);
	}

	public void ChildCallingOnMouseDrag (int id)
	{
		isSeedSelected = true;
		seedSelectedID = id;
		if (unlockedSeedIDs.Count >= 5) {// show switch page button only if seeds unlocked are more than 4
			switchButton.SetActive (false);	
		}
		fieldInfoButton.SetActive (false);
	}

	public void ToggleDisplayCropMenu ()
	{		
		transform.position = new Vector3 (-500, -500, 0);
	}
}
