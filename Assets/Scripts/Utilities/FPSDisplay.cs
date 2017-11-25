﻿using UnityEngine;
using System.Collections;
using TMPro;

public class FPSDisplay : MonoBehaviour {
    public bool enableFPS = true;
    public TextMeshProUGUI fpsText;
    float deltaTime = 0.0f;
    float msec;
    float fps;
    string text;

    void LateUpdate() {
        if (enableFPS && fpsText != null) {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            //msec = deltaTime * 1000.0f;
            fps = 1.0f / deltaTime;
            //text = string.Format ("{0:0.0} ms ({1:0.} fps)", msec, fps);
            //fpsText.text = text;
            fpsText.text = fps.ToString("F0");
        }
    }
}