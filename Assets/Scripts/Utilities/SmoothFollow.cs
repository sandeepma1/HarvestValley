using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public float cameraSmooth = 0.1f;
    public bool CameraClamp = false;
    public float minX, maxX = 0;
    public float minY, maxY = 0;
    private Transform player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.transform.position;
    }

    void Update()
    {
        if (player)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, cameraSmooth);
            transform.position = player.position + new Vector3(0, 0, -10);
        }
        if (CameraClamp)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY),
                -10);
        }
    }
}



//public Transform target;
//public float distance = 3.0f;
//public float height = 3.0f;
//public float damping = 5.0f;
//public bool followBehind = true;

//void Update()
//{
//    Vector3 wantedPosition;
//    if (followBehind)
//    {
//        wantedPosition = target.TransformPoint(0, height, -distance);
//    }
//    else
//    {
//        wantedPosition = target.TransformPoint(0, height, distance);
//    }

//    transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);
//}