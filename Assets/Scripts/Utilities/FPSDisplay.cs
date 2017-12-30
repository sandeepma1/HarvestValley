using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public bool enableFPS = true;
    public Text fpsText;
    float deltaTime = 0.0f;
    float fps;

    void Update()
    {
        if (enableFPS && fpsText != null)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            fps = 1.0f / deltaTime;
            fpsText.text = fps.ToString("F0");
        }
    }
}