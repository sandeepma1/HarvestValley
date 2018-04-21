using UnityEngine;
using HarvestValley.Ui;

public class FPSDisplay : MonoBehaviour
{
    //string label = "";
    //float count;

    //IEnumerator Start()
    //{
    //    GUI.depth = 2;
    //    while (true)
    //    {
    //        if (Time.timeScale == 1)
    //        {
    //            yield return new WaitForSeconds(0.1f);
    //            count = (1 / Time.deltaTime);
    //            label = "" + (Mathf.Round(count));
    //        } else
    //        {
    //            label = "Pause";
    //        }
    //        text.text = label;
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}
    float deltaTime;
    int w, h;
    GUIStyle style;
    Rect rect;
    float msec;
    float fps;
    string text;

    void Start()
    {
        deltaTime = 0.0f;

        w = Screen.width;
        h = Screen.height;
        rect = new Rect(0, 0, w, h);
        style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = ColorConstants.FpsColor;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.LowerCenter;
    }
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
        text = (msec.ToString("F0") + "ms " + fps.ToString("F0"));
        GUI.Label(rect, text, style);
    }
}
