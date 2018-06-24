using System;
using UnityEngine;
using UnityEngine.UI;

public class UiMinesDecendMenu : Singleton<UiMinesDecendMenu>
{
    public event Action OnYesButtonClicked;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;

    private void Start()
    {
        yesButton.onClick.AddListener(YesButtonClicked);
        noButton.onClick.AddListener(HideThisMenu);
        HideThisMenu();
    }

    protected override void OnDestroy()
    {
        yesButton.onClick.RemoveListener(YesButtonClicked);
        noButton.onClick.RemoveListener(HideThisMenu);
    }

    private void YesButtonClicked()
    {
        OnYesButtonClicked.Invoke();
    }

    public void ShowThisMenu()
    {
        mainMenu.SetActive(true);
    }
    public void HideThisMenu()
    {
        mainMenu.SetActive(false);
        PlayerController.Instance.isPlayerInAction = false;
    }
}
