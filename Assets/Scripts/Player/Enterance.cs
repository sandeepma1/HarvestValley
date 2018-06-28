using UnityEngine;

public class Enterance : MonoBehaviour
{
    private void Start()
    {
        PlayerController.Instance.OnEnteranceClicked += OnEnteranceClickedEventhandler;
        if (UiMinesDecendMenu.Instance == null)
        {
            UiMinesDecendMenu.Instance.OnYesButtonClicked += OnYesButtonClickedEventHandler;
        }
    }

    private void OnYesButtonClickedEventHandler()
    {
        GEM.currentMinesLevel++;
        SceneChanger.Instance.LoadScene("Mines");
    }

    private void OnEnteranceClickedEventhandler(EnteranceType enteranceType, int mineLevelNumber)
    {
        print("enteranceType " + enteranceType);
        PlayerController.Instance.isPlayerInAction = true;
        switch (enteranceType)
        {
            case EnteranceType.None:
                break;
            case EnteranceType.Home:
                SceneChanger.Instance.LoadScene("Main");
                break;
            case EnteranceType.Mines:
                GEM.currentMinesLevel = 0;
                SceneChanger.Instance.LoadScene("Mines");
                break;
            case EnteranceType.MinesLowerLevel:
                UiMinesDecendMenu.Instance.ShowThisMenu();
                break;
            case EnteranceType.MinesSelectedLevel:
                GEM.currentMinesLevel = mineLevelNumber;
                SceneChanger.Instance.LoadScene("Mines");
                break;
            case EnteranceType.Village:
                SceneChanger.Instance.LoadScene("Village");
                break;
            default:
                print(enteranceType + " might not be implemented");
                break;
        }
    }
}