using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FirstScript : MonoBehaviour
{
	public static FirstScript m_instance = null;
	public GameObject inventoryMenu, mainCanvas, seedMenu, FarmlandTimer, DummyfarmLoand;
	// Use this for initialization
	public Hashtable ease = new Hashtable ();

	void Awake ()
	{
		m_instance = this;
		if (mainCanvas != null) {
			mainCanvas.SetActive (true);
		}
		if (inventoryMenu != null) {
			inventoryMenu.SetActive (true);
		}
		if (seedMenu != null) {
			seedMenu.SetActive (false);
		}
		if (FarmlandTimer != null) {
			FarmlandTimer.SetActive (false);
		}
		if (DummyfarmLoand != null) {
			DummyfarmLoand.SetActive (false);
		}

		ease.Add ("ease", LeanTweenType.easeOutExpo);
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
		if (Input.GetMouseButtonDown (1)) {
			PlayerPrefs.SetInt ("firstFarms", 0);
			print (PlayerPrefs.GetInt ("firstFarms"));
			SceneManager.LoadScene ("Main");
		}
		if (Input.GetMouseButtonDown (2))
			SceneManager.LoadScene ("Main");		
	}
}
