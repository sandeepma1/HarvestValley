using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestMenuManager : MonoBehaviour
{
	public static HarvestMenuManager m_instance = null;
	public bool isScytheSelected = false;

	void Awake ()
	{
		m_instance = this;
	}

	public void ToggleDisplayHarvestingMenu ()
	{
		transform.position = new Vector3 (-500, -500, 0);
	}
}
