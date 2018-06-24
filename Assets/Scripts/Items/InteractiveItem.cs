using UnityEngine;

public class InteractiveItem : MonoBehaviour
{
    //[HideInInspector]
    public InteractiveItemType interactiveItemType = InteractiveItemType.None;
    //[HideInInspector]
    public EnteranceType enterance = EnteranceType.None;
    //[HideInInspector]
    public OpenMenuTypes openMenu = OpenMenuTypes.None;
    //[HideInInspector]
    public int itemId = -1;
    //[HideInInspector]
    public int buildingId = -1;
    //[HideInInspector]
    public int minesLevel = -1;
}