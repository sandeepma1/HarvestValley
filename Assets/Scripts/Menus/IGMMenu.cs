using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IGMMenu : MonoBehaviour
{
	public static IGMMenu m_instance = null;

	public GameObject loadingScreen;
	public GameObject mainCanvas;
	public GameObject[] disableAllMenus;
	//public GameObject[] setPositionFar;
	public Hashtable ease = new Hashtable ();

	void Awake ()
	{
		m_instance = this;
		if (mainCanvas != null) {
			mainCanvas.SetActive (true);
		}
	}

	void Start ()
	{
		DisableAllMenus ();
		ease.Add ("ease", LeanTweenType.easeOutSine);
	}

	public void DisableAllMenus ()
	{
		for (int i = 0; i < disableAllMenus.Length; i++) {			
			disableAllMenus [i].transform.position = new Vector3 (-500, -500, 0);
			disableAllMenus [i].SetActive (true);
		}
		BuildingsManager.m_instance.DisableAnyOpenMenus ();
	}

	public void ChangeCamera (float pos)
	{
		LeanTween.moveX (Camera.main.gameObject, pos, 0.5f, ease);
	}

	public void LoadScene (string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}

	public void QuitGame ()
	{
		Application.Quit ();
	}

	public void CloseGameObject (GameObject go)
	{
		go.SetActive (false);
	}

	public void OpenGameObject (GameObject go)
	{
		go.SetActive (true);
	}

	void Update ()
	{		
		/*if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) {
			if (Input.GetKey (KeyCode.Escape)) {
				print ("escape");
				if (SceneManager.GetActiveScene ().name == "Menu") {
					Application.Quit ();
				} else {
					//ShowIGMMenu ();
				}
				return;
			}
		}*/
	}




}
