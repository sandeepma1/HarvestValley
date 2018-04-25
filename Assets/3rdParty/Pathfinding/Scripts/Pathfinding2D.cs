using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Pathfinding2D : MonoBehaviour
{
    public List<Vector3> Path = new List<Vector3>();
    public bool JS = false;
    public Vector3 clickPoint;

    public void FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Pathfinder2D.Instance.InsertInQueue(startPosition, endPosition, SetList);
    }

    public void FindJSPath(Vector3[] arr)
    {
        if (arr.Length > 1)
        {
            Pathfinder2D.Instance.InsertInQueue(arr[0], arr[1], SetList);
        }
    }

    //A test move function, can easily be replaced
    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Path[0], Time.deltaTime * 10F);
        if (Vector3.Distance(transform.position, Path[0]) < 0.1F)
        {
            Path.RemoveAt(0);
        }
    }

    protected virtual void SetList(List<Vector3> path)
    {
        if (path == null)
        {
            return;
        }

        if (!JS)
        {
            Path.Clear();
            Path = path;
            Path[0] = new Vector3(Path[0].x, Path[0].y, Path[0].z);
            Path[Path.Count - 1] = new Vector3(Path[Path.Count - 1].x, Path[Path.Count - 1].y, Path[Path.Count - 1].z);
            int last = Path.Count - 1;
            UiDebugTextHandler.DebugText(Pathfinder2D.Instance.IsEndNodeWalkable(clickPoint).ToString());
            if (Pathfinder2D.Instance.IsEndNodeWalkable(clickPoint))
            {
                Path.RemoveAt(Path.Count - 1);
                Path.RemoveAt(Path.Count - 1);
            }
        }
        else
        {
            Vector3[] arr = new Vector3[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                arr[i] = path[i];
            }

            arr[0] = new Vector3(arr[0].x, arr[0].y, arr[0].z);
            arr[arr.Length - 1] = new Vector3(arr[arr.Length - 1].x, arr[arr.Length - 1].y, arr[arr.Length - 1].z);
            gameObject.SendMessage("GetJSPath", arr);
        }
    }
}
