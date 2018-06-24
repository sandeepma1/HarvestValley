using UnityEngine;

public class OpenMenu : MonoBehaviour
{

    private void Start()
    {
        PlayerController.Instance.OnOpenMenuClicked += OnOpenMenuClickedEventhandler;
    }

    private void OnOpenMenuClickedEventhandler(OpenMenuTypes openMenuType)
    {
        //PlayerController.Instance.isPlayerInAction = true;
        switch (openMenuType)
        {
            case OpenMenuTypes.None:
                break;
            case OpenMenuTypes.MineLevelSelector:
                UiMinesLevelSelectionMenu.Instance.ShowThisMenu();
                break;
            case OpenMenuTypes.SomeXyz:
                break;
            default:
                break;
        }
    }
}
