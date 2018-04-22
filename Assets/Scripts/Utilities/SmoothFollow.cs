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