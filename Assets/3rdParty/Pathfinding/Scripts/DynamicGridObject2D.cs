using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGridObject2D : MonoBehaviour
{

    private List<Vector2> IDs = new List<Vector2>();

    public float lowestY = 0F;
    public float timer = 0F;
    public bool SetTimer = false;

    private Vector3 lastPos = Vector3.zero;
    private Quaternion lastRot = Quaternion.identity;

    void Start()
    {
        StartCoroutine(DelayStart());
    }

    void Update()
    {
        if (!SetTimer)
        {
            if (transform.position != lastPos || transform.rotation != lastRot)
            {
                lastPos = transform.position;
                lastRot = transform.rotation;
                RemoveFromMap();
                UpdateMap();
            }
        }
    }

    void OnDestroy()
    {
        RemoveFromMap();
    }

    public void UpdateMap()
    {
        List<Vector2> checkList = new List<Vector2>();
        Bounds bR = GetComponent<Renderer>().bounds;

        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;

        if (transform.localScale.x % 2 == 0)
        {
            minX = bR.min.x;
            maxX = bR.max.x;
        }
        else
        {
            minX = bR.min.x;
            maxX = bR.max.x;
        }
        if (transform.localScale.y % 2 == 0)
        {
            minY = bR.min.y;
            maxY = bR.max.y;
        }
        else
        {
            minY = bR.min.y;
            maxY = bR.max.y;
        }

        checkList = DynamicSetupList(minX, maxX, minY, maxY);
        //checkList = DynamicSetupList(bR.min.x, bR.max.x, bR.min.y, bR.max.y);
        Pathfinder2D.Instance.DynamicMapEdit(checkList, UpdateList);
    }

    public void RemoveFromMap()
    {
        if (IDs != null)
        {
            Pathfinder2D.Instance.DynamicRedoMapEdit(IDs);
        }
    }

    private void UpdateList(List<Vector2> ids)
    {
        IDs = ids;
    }

    private List<Vector2> DynamicSetupList(float minX, float maxX, float minY, float maxY)
    {
        List<Vector2> checkList = new List<Vector2>();
        float Tilesize = Pathfinder2D.Instance.Tilesize;
        for (float i = minX; i < maxX; i++)
        {
            for (float j = minY; j < maxY; j++)
            {
                checkList.Add(new Vector2(i, j));
            }
        }
        return checkList;
    }

    IEnumerator CoroutineUpdate(float _timer)
    {
        if (transform.position != lastPos || transform.rotation != lastRot)
        {
            lastPos = transform.position;
            lastRot = transform.rotation;
            RemoveFromMap();
            UpdateMap();
        }

        //Wait amount of time and call its self recursively
        yield return new WaitForSeconds(_timer);
        StartCoroutine(CoroutineUpdate(_timer));
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForEndOfFrame();

        lastPos = transform.position;
        lastRot = transform.rotation;
        UpdateMap();

        if (SetTimer)
        {
            StartCoroutine(CoroutineUpdate(0.2f)); //Calls it 5 times per second
        }
    }
}
