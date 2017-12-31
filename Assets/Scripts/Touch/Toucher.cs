using UnityEngine;

public class Toucher : MonoBehaviour
{
    public void TouchUp()
    {
        print("TouchUp");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print("OnCollisionEnter2D " + other.gameObject.name);
    }
}