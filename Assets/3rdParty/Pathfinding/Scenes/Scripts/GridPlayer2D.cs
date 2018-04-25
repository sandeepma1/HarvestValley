using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridPlayer2D : Pathfinding2D
{
    [SerializeField]
    private Transform pointer;
    void Update()
    {
        FindPath();
        if (Path.Count > 0)
        {
            Move();
        }
    }

    private void FindPath()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //hit.point = new Vector3(RoundToNearestHalfSafer(hit.point.x), RoundToNearestHalfSafer(hit.point.y), Mathf.Round(hit.point.z));
                hit.point = new Vector3(Mathf.Round(hit.point.x), Mathf.Round(hit.point.y), Mathf.Round(hit.point.z));
                pointer.transform.position = hit.point;
                FindPath(transform.position, hit.point);
            }
        }
    }
}
