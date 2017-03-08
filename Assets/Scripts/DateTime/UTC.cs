using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTC : MonoBehaviour
{
	public static UTC time = null;
	public System.DateTime DateTimeNow;

	void Awake ()
	{
		DateTimeNow = System.DateTime.UtcNow;
		time = this;
		InvokeRepeating ("MaintainDateTime", 0, 1);
	}

	void MaintainDateTime ()
	{
		DateTimeNow = System.DateTime.UtcNow;
	}
}
