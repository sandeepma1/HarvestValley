using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiRoot : MonoBehaviour
{
    private void Awake()
    {
        Camera mainCamera = Camera.main;
        Canvas[] allCanvas = GetComponentsInChildren<Canvas>();
        for (int i = 0; i < allCanvas.Length; i++)
        {
            allCanvas[i].worldCamera = mainCamera;
        }
    }
}
