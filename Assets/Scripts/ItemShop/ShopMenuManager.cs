using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;

public class ShopMenuManager : MonoBehaviour
{
	public static ShopMenuManager m_instance = null;
	public GameObject shopUIMenu, shopToggleButton, shopitemPrefab, UIList;
	public GameObject dragImageSprite;
	public ShopItem[] shopItems;
	public GameObject[] shopItemsGO;
	bool showShop = false;

	void Awake ()
	{
		m_instance = this;
		//ToggleShopView ();
		Initialize ();
		PopulateScrollListAtStart ();
	}

	#region CSV to Array

	void Initialize ()
	{
		string[] lines = new string[100];
		string[] chars = new string[100];
		TextAsset itemCSV =	Resources.Load ("CSVs/Shop") as TextAsset;
		lines = Regex.Split (itemCSV.text, "\r\n");
		shopItems = new ShopItem[lines.Length - 2];
		for (int i = 1; i < lines.Length - 1; i++) {			
			chars = Regex.Split (lines [i], ",");
			shopItems [i - 1] = new ShopItem (IntParse (chars [0]), chars [1], chars [2]);
		}

		print (shopItems.Length);
	}

	int IntParse (string text)
	{
		int num;
		if (int.TryParse (text, out num)) {
			return num;
		} else
			return 0;
	}

	float FloatParse (string text)
	{
		float result = 0.01f;
		float.TryParse (text, out result);
		return result;

	}

	#endregion

	public void PopulateScrollListAtStart ()
	{
		// Image, background, name, desc
		shopItemsGO = new GameObject[shopItems.Length];
		for (int i = 0; i < shopItems.Length; i++) {
			shopItemsGO [i] = Instantiate (shopitemPrefab, UIList.transform);
			shopItemsGO [i].transform.GetChild (0).GetComponent <DraggableItemShopUIItem> ().shopItemID = i;
			shopItemsGO [i].transform.GetChild (0).GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> ("Textures/Buildings/" + shopItems [i].shopItemName);
			shopItemsGO [i].transform.GetChild (2).GetComponent<TextMeshProUGUI> ().text = shopItems [i].shopItemName.ToString ();
			shopItemsGO [i].transform.GetChild (3).GetComponent<TextMeshProUGUI> ().text = shopItems [i].shopItemDesc.ToString ();
		}
	}

	public 	void ChildCallingOnMouseDrag (int shopItemID)
	{
		dragImageSprite.GetComponent <SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Textures/Buildings/" + shopItems [shopItemID].shopItemName);
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
			shopToggleButton.GetComponent <RectTransform> ().anchoredPosition = new Vector2 (-50, 150);
		}

	}
}

public class ShopItem
{
	public int	shopItemID;
	public string shopItemName;
	public string shopItemDesc;
	//public int shopitemCost;
	//shopItemType
	//public int shopitemLimit

	public ShopItem (int id, string name, string desc)
	{
		shopItemID = id;
		shopItemName = name;
		shopItemDesc = desc;
	}
}
