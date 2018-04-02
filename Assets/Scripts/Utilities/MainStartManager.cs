using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStartManager : MonoBehaviour
{
    private Canvas[] canvas;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        canvas = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].worldCamera = mainCamera;
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyUp(KeyCode.R))
        {
            PlayerPrefs.SetInt("gameStatus", 0);
            SceneManager.LoadScene("Start");
        }
    }
}
