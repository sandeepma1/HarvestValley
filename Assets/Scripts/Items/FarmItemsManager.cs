using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FarmItemsManager : MonoBehaviour
{
	public GameObject ListPrefab, ScrollListGO;
	public static FarmItemsManager m_instance = null;
	public List <int> playerItems = new List<int> ();
	GameObject[] listItem;

	void Awake ()
	{
		m_instance = this;
		NewGameStart ();
		playerItems = ES2.LoadList<int> ("playerInventory");
		listItem = new GameObject[playerItems.Count];
		PopulateScrollList ();
	}

	void PopulateScrollList ()
	{
		for (int i = 0; i < playerItems.Count; i++) {
			listItem [i] = Instantiate (ListPrefab, ScrollListGO.transform);
			listItem [i].transform.GetChild (0).GetComponent <TextMeshProUGUI> ().text = playerItems [i].ToString ();
			listItem [i].GetComponent <Image> ().overrideSprite = Resources.Load<Sprite> ("Textures/Crop/" + ItemDatabase.m_instance.items [i].name);
			listItem [i].name = "Item" + i;
		}
	}

	void UpdateScrollListItemCount ()
	{
		for (int i = 0; i < playerItems.Count; i++) {			
			listItem [i].transform.GetChild (0).GetComponent <TextMeshProUGUI> ().text = playerItems [i].ToString ();
		}
	}

	public void AddFarmItems (int id, int value)
	{		
		playerItems [id] += value;
		UpdateScrollListItemCount ();
		ES2.Save (playerItems, "playerInventory");
	}

	#region Init FarmStorage

	void NewGameStart ()
	{ 
		if (PlayerPrefs.GetInt ("playerInventory") <= 0) {			
			ES2.Delete ("playerInventory");
			playerItems.Add (4);
			playerItems.Add (4);
			playerItems.Add (4);
			playerItems.Add (4);
			ES2.Save (playerItems, "playerInventory");
			PlayerPrefs.SetInt ("playerInventory", 1);
		}
	}

	#endregion
}