using UnityEngine;
using UnityEngine.UI;

public class ScreenButtons : MonoBehaviour
{
    [SerializeField]
    private int id;

    private Button yourButton;

    void Start()
    {
        yourButton = GetComponent<Button>();
        yourButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        InputController.instance.SnapCameraOnButton(id);
    }
}
