using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMineScene : MonoBehaviour
{

	void OnMouseUp ()
	{
		SceneManager.LoadScene ("Mines");
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (1)) {
			PlayerPrefs.SetInt ("firstFarms", 0);
			print (PlayerPrefs.GetInt ("firstFarms"));
		}
	}
}
