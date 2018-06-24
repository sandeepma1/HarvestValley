using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiClickableMinesLevelButtons : MonoBehaviour
{
    public event Action<int> OnButtonClick;
    [SerializeField]
    private TextMeshProUGUI buttonNumbersText;
    private int levelNumber;
    public int LevelNumber
    {
        set
        {
            levelNumber = value;
            buttonNumbersText.text = "Level " + levelNumber.ToString();
        }
    }

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClickEventHandler);
    }

    private void OnButtonClickEventHandler()
    {
        OnButtonClick.Invoke(levelNumber);
    }
}