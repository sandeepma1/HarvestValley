using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    //public bool enableFPS = true;
    //public Text fpsText;
    //float deltaTime = 0.0f;
    //float fps;

    //void Update()
    //{
    //    if (enableFPS && fpsText != null)
    //    {
    //        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    //        fps = 1.0f / deltaTime;
    //        fpsText.text = fps.ToString("F0");
    //    }
    //}
    string label = "";
    float count;
    public Text text;

    IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            if (Time.timeScale == 1)
            {
                yield return new WaitForSeconds(0.1f);
                count = (1 / Time.deltaTime);
                label = "" + (Mathf.Round(count));
            } else
            {
                label = "Pause";
            }
            text.text = label;
            yield return new WaitForSeconds(0.5f);
        }
    }
}