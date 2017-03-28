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
		transform.position = Input.mousePosition;
	}

	public void OnUp ()
	{
		transform.localPosition = intialPosition;
		ShopMenuManager.m_instance.ChildCallingOnMouseUp (shopItemID);
	}
}
