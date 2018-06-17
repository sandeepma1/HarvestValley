using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Options")]
    [SerializeField]
    [Range(0f, 2f)] internal float handleLimit = 1f;

    internal Vector2 inputVector = Vector2.zero;

    [Header("Components")]
    [SerializeField]
    internal RectTransform background;
    [SerializeField]
    internal RectTransform handle;
    [SerializeField]
    internal Button actionButton;
    [SerializeField]
    private TextMeshProUGUI actionText;
    [SerializeField]
    internal Button secondaryButton;
    [SerializeField]
    private TextMeshProUGUI secondaryText;

    public bool ActionButtonVisiblilty
    {
        set
        {
            if (actionButton.gameObject.activeInHierarchy != value)
            {
                actionButton.gameObject.SetActive(value);
            }
        }
    }
    public bool SecondaryButtonVisiblilty { set { secondaryButton.gameObject.SetActive(value); } }
    public string ActionString { set { actionText.text = value; } }
    public string SecondaryString { set { secondaryText.text = value; } }
    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }
}
