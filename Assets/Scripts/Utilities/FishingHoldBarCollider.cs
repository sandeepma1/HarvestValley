using System;
using UnityEngine;

public class FishingHoldBarCollider : MonoBehaviour
{
    public bool isOnStay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FlappyFish"))
        {
            isOnStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FlappyFish"))
        {
            isOnStay = false;
        }
    }
}