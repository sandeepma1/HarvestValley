using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

[System.Serializable]
public class PlayerLevel
{
	public int XPforNextLevel;
	public int cropsUnlockID;
	public int animalsUnlockID;
	public int productsUnlockID;
	public int productionBuildingUnlockID;
}
