using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FirstScript : MonoBehaviour
{
	public GameObject inventoryMenu, mainCanvas, seedMenu;
	// Use this for initialization
	void Awake ()
	{	
		if (mainCanvas != null) {
			mainCanvas.SetActive (true);
		}
		if (inventoryMenu != null) {
			inventoryMenu.SetActive (true);
		}
		if (seedMenu != null) {
			seedMenu.SetActive (false);
		}
	}

	void Start ()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Main_SB") {
			SetupCamera ();
		}
	}

	void SetupCamera ()
	{
		Camera.main.transform.GetComponent <CameraFollow> ().minX = WarpManager.m_instance.warp [Bronz.LocalStore.Instance.GetInt ("mapChunkPosition")].camMinX;
		Camera.main.transform.GetComponent <CameraFollow> ().maxX = WarpManager.m_instance.warp [Bronz.LocalStore.Instance.GetInt ("mapChunkPosition")].camMaxX;
		Camera.main.transform.GetComponent <CameraFollow> ().minY = WarpManager.m_instance.warp [Bronz.LocalStore.Instance.GetInt ("mapChunkPosition")].camMinY;
		Camera.main.transform.GetComponent <CameraFollow> ().maxY = WarpManager.m_instance.warp [Bronz.LocalStore.Instance.GetInt ("mapChunkPosition")].camMaxY;
	}

	void LateUpdate ()
	{
		if (Input.GetMouseButtonDown (2))
			SceneManager.LoadScene ("Main");
	}
}
