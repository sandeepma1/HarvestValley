using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class PlacableTileManager : MonoBehaviour
{
	public static PlacableTileManager m_instance = null;
	public GameObject farmLandPrefab;
	public GameObject FarmLandMenu = null, FarmTimerText = null;

	bool toggleFarmLandMenu = false;
	List<SaveFarmLand> farmList = new List<SaveFarmLand> ();
	GameObject[] FarmLand;
	int farmLandCount = 0;

	void Awake ()
	{
		m_instance = this;
		OneTimeOnly ();
		Init ();
	}

	void Init ()
	{
		farmList = ES2.LoadList<SaveFarmLand> ("farmList");
		FarmLand = new GameObject[farmList.Count];
		foreach (var item in farmList) {
			InitFarmLands (item);
		}
		print (farmList.Count);
	}

	public void AddNewFarmLandInMap (int index)
	{
		print (System.DateTime.UtcNow.ToString ());
		farmList.Add (new SaveFarmLand ((sbyte)index, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
		GameObject FarmLand = Instantiate (farmLandPrefab, this.transform);
		FarmLand.transform.localPosition = new Vector3 (index, 0, 0);
		FarmLand.gameObject.name = "FarmLand" + index;
		ES2.Save (farmList, "farmList");
	}

	public void InitFarmLands (SaveFarmLand item)
	{				
		FarmLand [farmLandCount] = Instantiate (farmLandPrefab, this.transform);
		FarmLand [farmLandCount].transform.localPosition = new Vector3 (item.tileIndex, 0, 0);
		FarmLand [farmLandCount].gameObject.name = "FarmLand" + item.tileIndex;
		FarmLand [farmLandCount].GetComponent <FarmLands> ().tileIndex = item.tileIndex;
		FarmLand [farmLandCount].GetComponent <FarmLands> ().level = item.level;
		FarmLand [farmLandCount].GetComponent <FarmLands> ().seedIndex = item.seedIndex;
		FarmLand [farmLandCount].GetComponent <FarmLands> ().state = item.state;
		FarmLand [farmLandCount].GetComponent <FarmLands> ().dateTime = System.DateTime.Parse (item.dateTime);
		farmLandCount++;
	}

	public void CallParentOnMouseUp (int index)
	{
		FarmLandMenu.transform.position = FarmLand [index].transform.position;
		toggleFarmLandMenu = !toggleFarmLandMenu;
		if (toggleFarmLandMenu) {
			FarmLandMenu.SetActive (true);
		} else {
			FarmLandMenu.SetActive (false);
		}
	}

	#region OnMouse Functions

	void OneTimeOnly ()
	{ 
		if (PlayerPrefs.GetInt ("firstFarms") <= 0) {
			ES2.Delete ("farmList");
			farmList.Add (new SaveFarmLand (0, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (1, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			farmList.Add (new SaveFarmLand (2, 1, -1, 0, System.DateTime.UtcNow.ToString ()));
			ES2.Save (farmList, "farmList");
			PlayerPrefs.SetInt ("firstFarms", 1);
			print (PlayerPrefs.GetInt ("firstFarms") + " only once");
		}
	}

	public void CallParentOnMouseDown (int index)
	{	
		print ("Farm " + index + " MouseDown");
	}

	public void CallParentOnMouseEnter (int index)
	{
		print ("Farm " + index + " MouseEnter");
	}

	public void CallParentOnMouseDrag (int index)
	{
		print ("Farm " + index + " MouseDrag");
	}

	public void CallParentOnMouseExit (int index)
	{
		print ("Farm " + index + " MouseExit");
	}

	public void CallParentOnMouseUpAsButton (int index)
	{
		print ("Farm " + index + " MouseUpAsButton");
	}

	#endregion
}


