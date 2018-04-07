using UnityEngine;
using UnityEngine.UI;

public class UiDebugTextHandler : MonoBehaviour
{
    private static Text debugText;

    private void Start()
    {
        debugText = transform.GetComponentInChildren<Text>();
    }

    public static void DebugText(string text)
    {
        debugText.text = text;
    }
}