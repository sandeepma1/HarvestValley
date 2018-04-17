using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float cameraSmooth = 0.1f;
    public bool CameraClamp = false;
    public float minX, maxX = 0;
    public float minY, maxY = 0;

    // Use this for initialization
    void Start()
    {
        transform.position = target.transform.position;
        //mainCamera = GetComponent<Camera>();

    }

    void Update()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, cameraSmooth);
            // transform.position = target.position + new Vector3(0, 0, -10);
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