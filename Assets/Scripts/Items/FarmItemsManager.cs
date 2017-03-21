using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FarmItemsManager : MonoBehaviour
{
	public GameObject ListPrefab, ScrollListGO;
	public static FarmItemsManager m_instance = null;
	public List <FarmItems> playerItems = new List<FarmItems> ();

	List<GameObject> listItems = new List<GameObject> ();

	void Awake ()
	{
		m_instance = this;
		NewGameStart ();
		playerItems = ES2.LoadList<FarmItems> ("playerInventory");
		PopulateScrollList ();
		InvokeRepeating ("SavePlayerInventory", 3, 3);
	}

	public void PopulateScrollList ()
	{		
		for (int i = 0; i < playerItems.Count; i++) {
			AddOneItemInScrollList (i);
		}
	}

	void AddOneItemInScrollList (int scrollListID)
	{
		listItems.Add (Instantiate (ListPrefab, ScrollListGO.transform));

		listItems [scrollListID].GetComponent <RectTransform> ().localScale = Vector3.one;
		listItems [scrollListID].transform.GetChild (0).GetComponent <TextMeshProUGUI> ().text = playerItems [scrollListID].count.ToString ();
		listItems [scrollListID].GetComponent <Image> ().overrideSprite = Resources.Load<Sprite> ("Textures/Crop/" + ItemDatabase.m_instance.items [scrollListID].name);
		listItems [scrollListID].name = "Item" + scrollListID;
	}

	public void UpdateScrollListItemCount ()
	{
		for (int i = 0; i < playerItems.Count; i++) {		
			listItems [i].transform.GetChild (0).GetComponent <TextMeshProUGUI> ().text = playerItems [i].count.ToString ();
		}
		//ES2.Save (playerItems, "playerInventory");
	}

	public void UpdateFarmItems (int id, int value)
	{		
		foreach (var item in playerItems) {
			if (item.id == id) {
				item.count += value;
			}
		}
		UpdateScrollListItemCount ();
	}

	public void AddNewFarmItem (int id, int value)
	{		
		playerItems.Add (new FarmItems (id, value));
		AddOneItemInScrollList (playerItems.Count - 1);
		UpdateScrollListItemCount ();
	}

	void SavePlayerInventory ()
	{
		ES2.Save (playerItems, "playerInventory");
	}

	#region Init FarmStorage

	void NewGameStart ()
	{ 
		if (PlayerPrefs.GetInt ("playerInventory") <= 0) {			
			ES2.Delete ("playerInventory");
			foreach (var item in ItemDatabase.m_instance.items) {				
				if (item.unlocksAtLevel <= PlayerProfileManager.m_instance.CurrentPlayerLevel ()) {					
					playerItems.Add (new FarmItems (item.id, 4));
				}
			}
			ES2.Save (playerItems, "playerInventory");
			PlayerPrefs.SetInt ("playerInventory", 1);
		}
	}

	#endregion
}

public class FarmItems
{
	public int id;
	public int count;

	public FarmItems (int i_id, int i_count)
	{
		id = i_id;
		count = i_count;
	}

	public FarmItems ()
	{
		
	}
}