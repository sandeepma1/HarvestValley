using UnityEngine;

public class UTC : MonoBehaviour
{
    public System.DateTime liveDateTime;

    void Awake()
    {
        liveDateTime = System.DateTime.UtcNow;
    }

    void FixedUpdate()
    {
        liveDateTime = System.DateTime.UtcNow;
    }
}
