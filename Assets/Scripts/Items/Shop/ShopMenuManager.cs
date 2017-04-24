using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
using TMPro;

public class ShopMenuManager : MonoBehaviour
{
	public static ShopMenuManager m_instance = null;
	public GameObject shopUIMenu, shopToggleButton, shopitemPrefab, UIList;
	public GameObject dragImageSprite;

	public GameObject[] shopItemsGO;
	bool showShop = false;

	void Awake ()
	{
		m_instance = this;
		PopulateScrollListAtStart ();
	}

	public void PopulateScrollListAtStart ()
	{
		// Image, background, name, desc
		shopItemsGO = new GameObject[BuildingDatabase.m_instance.buildingInfo.Length];
		for (int i = 0; i < BuildingDatabase.m_instance.buildingInfo.Length; i++) {
			shopItemsGO [i] = Instantiate (shopitemPrefab, UIList.transform);
			shopItemsGO [i].transform.GetChild (0).GetComponent <DraggableItemShopUIItem> ().shopItemID = i;
			shopItemsGO [i].transform.GetChild (0).GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> ("Textures/Buildings/" + BuildingDatabase.m_instance.buildingInfo [i].name);
			shopItemsGO [i].transform.GetChild (2).GetComponent<TextMeshProUGUI> ().text = BuildingDatabase.m_instance.buildingInfo [i].name.ToString ();
			shopItemsGO [i].transform.GetChild (3).GetComponent<TextMeshProUGUI> ().text = BuildingDatabase.m_instance.buildingInfo [i].desc.ToString ();
			shopItemsGO [i].transform.GetChild (4).GetComponent<TextMeshProUGUI> ().text = BuildingDatabase.m_instance.buildingInfo [i].cost.ToString ();
		}
	}

	public 	void ChildCallingOnMouseDrag (int shopItemID)
	{
		dragImageSprite.GetComponent <SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Textures/Buildings/" + BuildingDatabase.m_instance.buildingInfo [shopItemID].name);
		dragImageSprite.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
	}

	public void ChildCallingOnMouseUp (int shopItemID)
	{
		dragImageSprite.transform.position = new Vector3 (500, 500, 0);
	}

	public void ToggleShopView ()
	{
		showShop = !showShop;
		if (showShop) {  //show shop
			shopUIMenu.GetComponent <RectTransform> ().anchoredPosition = new Vector2 (0, 175);
			shopToggleButton.GetComponent <RectTransform> ().anchoredPosition = new Vector2 (-50, 50);
		} else {
			shopUIMenu.GetComponent <RectTransform> ().anchoredPosition = new Vector2 (0, -175);
			shopToggleButton.GetComponent <RectTransform> ().anchoredPosition = new Vector2 (-50, 120);
		}
	}
}

