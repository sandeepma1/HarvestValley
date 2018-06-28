using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private float pickaxeHitDuration = 0.25f;  //Todo make upgradable pickaxe/tools
    private PickaxeAble thisPickaxeAble = null;

    private void Start()
    {
        PlayerController.Instance.OnPickaxeAbleClicked += OnPickaxeClickedEventhandler;
    }

    private void OnPickaxeClickedEventhandler(PickaxeAble pickaxeAble)
    {
        thisPickaxeAble = pickaxeAble;
        PlayerController.Instance.isPlayerInAction = true;
        PlayerMovement.Instance.PlayerPickaxeAction(true);
        Invoke("HitPickaxeAble", pickaxeHitDuration);
    }

    private void HitPickaxeAble()
    {
        thisPickaxeAble.HitPoints -= 2;
        PlayerMovement.Instance.PlayerPickaxeAction(false);
        PlayerController.Instance.isPlayerInAction = false;
    }
}