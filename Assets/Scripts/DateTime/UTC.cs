using UnityEngine;

public class UTC : MonoBehaviour
{
    public static UTC time = null;
    public System.DateTime liveDateTime;

    void Awake()
    {
        liveDateTime = System.DateTime.UtcNow;
        time = this;
    }

    void FixedUpdate()
    {
        liveDateTime = System.DateTime.UtcNow;
    }
}
