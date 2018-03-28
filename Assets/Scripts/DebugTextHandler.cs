using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugTextHandler : Singleton<DebugTextHandler>
{
    Text debugText;

    void Start()
    {
        debugText = transform.GetComponent<Text>();
    }

    public void DisplayDebugText(string text)
    {
        debugText.text = text;
    }

    public void Dropped()
    {
        print("dropped");
    }

    public void Grabbed()
    {
        print("Grabbed");
    }

    public void Removed()
    {
        print("Removed");
    }

    public void Changed(Vector2 pos)
    {
        print("Changed" + pos);
    }
}
