using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBackpackMenu : MonoBehaviour
{
    [SerializeField]
    private Button backpackButton;
    [SerializeField]
    private GameObject backpackCanvas;
    private bool showBackpack = false;

    private void Start()
    {
        backpackButton.onClick.AddListener(ToggleBackpackCanvas);
        backpackCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        backpackButton.onClick.RemoveListener(ToggleBackpackCanvas);
    }

    private void ToggleBackpackCanvas()
    {
        showBackpack = !showBackpack;
        backpackCanvas.SetActive(showBackpack);
    }

    public void CloseThisMenu()
    {
        ToggleBackpackCanvas();
    }
}
