using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInventory : MonoBehaviour
{



}

[System.Serializable]
public class MyItems
{
	public int id;
	public int count;

	public MyItems (int m_Id, int m_count)
	{
		id = m_Id;
		count = m_count;
	}
}