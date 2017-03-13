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
	
	GameObject[] listItem;

	void Awake ()
	{
		m_instance = this;
		NewGameStart ();
		playerItems = ES2.LoadList<FarmItems> ("playerInventory");
		listItem = new GameObject[playerItems.Count];
		PopulateScrollList ();
	}

	void PopulateScrollList ()
	{
		for (int i = 0; i < playerItems.Count; i++) {
			listItem [i] = Instantiate (ListPrefab, ScrollListGO.transform);
			listItem [i].transform.GetChild (0).GetComponent <TextMeshProUGUI> ().text = playerItems [i].count.ToString ();
			listItem [i].GetComponent <Image> ().overrideSprite = Resources.Load<Sprite> ("Textures/Crop/" + ItemDatabase.m_instance.items [i].name);
			listItem [i].name = "Item" + i;
		}
	}

	void UpdateScrollListItemCount ()
	{
		for (int i = 0; i < playerItems.Count; i++) {			
			listItem [i].transform.GetChild (0).GetComponent <TextMeshProUGUI> ().text = playerItems [i].count.ToString ();
		}
	}

	public void AddFarmItems (int id, int value)
	{		
		foreach (var item in playerItems) {
			if (item.id == id) {
				item.count += value;
			}
		}
		UpdateScrollListItemCount ();
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