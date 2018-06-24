using UnityEngine;
using System.Collections;

public class Pickaxe : MonoBehaviour
{
    private void Start()
    {
        PlayerController.Instance.OnPickaxeAbleClicked += OnPickaxeClickedEventhandler;
    }

    private void OnPickaxeClickedEventhandler(PickaxeAble pickaxeAble)
    {
        pickaxeAble.HitPoints -= 2;
        PlayerMovement.Instance.PlayerToolAction(false);
    }
}