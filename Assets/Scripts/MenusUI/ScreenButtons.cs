using UnityEngine;
using UnityEngine.UI;

public class ScreenButtons : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private float position;

    private Button yourButton;

    void Start()
    {
        yourButton = GetComponent<Button>();
        yourButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        InputController.Instance.SnapCameraOnButton(id);//, position);
    }
}
