using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackitemReceiver : MonoBehaviour
{
	public int id;

	public void OnClickEnter ()
	{		
		BuildingsManager.m_instance.CallParentOnMouseEnter (MasterMenuManager.m_instance.itemSelectedID);
	}
}
