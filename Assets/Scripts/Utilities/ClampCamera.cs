using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampCamera : MonoBehaviour
{
    [SerializeField]
    private bool cameraClamp = false;
    [SerializeField]
    private float minX, maxX = 0;
    [SerializeField]
    private float minY, maxY = 0;

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraClamp)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY),
                transform.position.z);
        }
    }
}
