using UnityEngine;

public class UTC : MonoBehaviour
{
    public System.DateTime liveDateTime;

    void Awake()
    {
        liveDateTime = System.DateTime.Now;
    }

    void FixedUpdate()
    {
        liveDateTime = System.DateTime.Now;
    }
}
