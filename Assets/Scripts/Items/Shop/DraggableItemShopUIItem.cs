using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItemShopUIItem : MonoBehaviour
{
	public int shopItemID = 0;
	Vector3 intialPosition;

	public void OnDown ()
	{
		intialPosition = transform.localPosition;
	}

	public void OnDrag ()
	{	
		ShopMenuManager.m_instance.ChildCallingOnMouseDrag (shopItemID);
		transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition) - BuildingsManager.m_instance.transform.position;
	}

	public void OnUp ()
	{
		transform.localPosition = intialPosition;
		print (Camera.main.ScreenToWorldPoint (new Vector3 (Mathf.RoundToInt (Input.mousePosition.x), Mathf.RoundToInt (Input.mousePosition.y)))
		- BuildingsManager.m_instance.transform.position);
		ShopMenuManager.m_instance.ChildCallingOnMouseUp (shopItemID, Camera.main.ScreenToWorldPoint (new Vector3 (Mathf.RoundToInt (Input.mousePosition.x), Mathf.RoundToInt (Input.mousePosition.y)))
		- BuildingsManager.m_instance.transform.position);
	}
}
