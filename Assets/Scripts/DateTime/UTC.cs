using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTC : MonoBehaviour
{
	public static UTC time = null;
	public System.DateTime liveDateTime;

	void Awake ()
	{
		liveDateTime = System.DateTime.UtcNow;
		time = this;
		//InvokeRepeating ("MaintainDateTime", 0, 0.25f);
	}

	void MaintainDateTime ()
	{
		liveDateTime = System.DateTime.UtcNow;
	}

	void FixedUpdate ()
	{
		liveDateTime = System.DateTime.UtcNow;
	}
}
