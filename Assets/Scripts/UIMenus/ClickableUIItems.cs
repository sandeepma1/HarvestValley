using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ClickableUIItems : MonoBehaviour
{
    public int itemID;
    public TextMeshProUGUI itemCostText;
    public TextMeshProUGUI itemNameText;
    public Image itemImage;
    public int selectedItemID;

    private Button button;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked()
    {
        UIMasterMenuManager.Instance.OnUIItemClicked(itemID);
        selectedItemID = -1;
    }
}
