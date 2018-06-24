using UnityEngine;

public class UiMinesLevelSelectionMenu : Singleton<UiMinesLevelSelectionMenu>
{
    [SerializeField]
    private UiClickableMinesLevelButtons uiClickableButtonsPrefab;
    [SerializeField]
    private Transform contentPanel;
    [SerializeField]
    private Transform mainMenu;

    private int levelCap = 5;
    private int latestLevel = 31; //TODO: create mines level saving.

    private void Start()
    {
        FillPanelWithButtons();
    }

    private void FillPanelWithButtons()
    {
        int numberOfButtons = latestLevel / levelCap;
        for (int i = 0; i < numberOfButtons; i++)
        {
            UiClickableMinesLevelButtons buttons = Instantiate(uiClickableButtonsPrefab, contentPanel);
            if (i == 0)
            {
                buttons.LevelNumber = 1;
            }
            else
            {
                buttons.LevelNumber = i * levelCap;
            }

            buttons.OnButtonClick += OnButtonClickEventHandler;
        }
        Destroy(uiClickableButtonsPrefab.gameObject);
    }

    private void OnButtonClickEventHandler(int levelNumber)
    {
        GEM.currentMinesLevel = levelNumber;
        SceneChanger.Instance.LoadScene("Mines");
    }

    public void CloseThisMenu()
    {
        mainMenu.gameObject.SetActive(false);
    }

    public void ShowThisMenu()
    {
        mainMenu.gameObject.SetActive(true);
    }
}
