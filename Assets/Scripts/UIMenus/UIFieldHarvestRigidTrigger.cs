
using UnityEngine;
using UnityEngine.UI;

public class UIFieldHarvestRigidTrigger : MonoBehaviour
{
    [SerializeField]
    private Image[] cropImage;

    private void OnEnable()
    {
        for (int i = 0; i < cropImage.Length; i++)
        {
            cropImage[i].gameObject.SetActive(true);
        }
    }

    // Todo: Bring number of crops yield and sprite
    public void ShowHarvesableCrops(int nos, Sprite cropSprite)
    {
        if (nos > 9)
        {
            nos = 9;
        }
        for (int i = 0; i < nos; i++)
        {
            cropImage[i].sprite = cropSprite;
            cropImage[i].gameObject.SetActive(true);
        }
    }
}