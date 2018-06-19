using UnityEngine;

public class Enterance : MonoBehaviour
{
    private void Start()
    {
        PlayerController.Instance.OnEnteranceClicked += OnEnteranceClickedEventhandler;
    }

    private void OnEnteranceClickedEventhandler(string enteranceName)
    {
        switch (enteranceName)
        {
            case "Mines":
                SceneChanger.Instance.LoadScene("Mines");
                break;
            case "ExitMines":
                SceneChanger.Instance.LoadScene("Main");
                break;
            default:
                print(enteranceName + " might not be implemented");
                break;
        }
    }
}