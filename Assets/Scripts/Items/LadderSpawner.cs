using UnityEngine;

public class LadderSpawner : MonoBehaviour
{
    private CircleCollider2D collider2D;

    private void Start()
    {
        collider2D = GetComponent<CircleCollider2D>();
        collider2D.enabled = false;
        Invoke("EnableCollider", 1.5f);
    }

    private void EnableCollider()
    {
        collider2D.enabled = true;
    }
}