using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UFO : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Points that are used for random movement")]
    private List<Transform> wayPoints = null;

    [SerializeField]
    [Tooltip("Points where the ufos can end at off screen")]
    private List<Transform> endPoints = null;

    [SerializeField]
    [Tooltip("Minimum way points")]
    private int minimumWayPoints = 4;

    [SerializeField]
    [Tooltip("Speed of the Ufo")]
    private float speed = 15.0f;

    Tween t;

    private void Start()
    {
        t = transform.DOPath(ReturnPathPoints(), speed, PathType.Linear, PathMode.Ignore)
            .SetAutoKill(false)
            .SetEase(Ease.Linear)
            .OnComplete( () => Debug.Log("Puff") );
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            t.Restart();
        }
    }

    //Generates an array of way points the ufo should take
    private Vector3[] ReturnPathPoints()
    {
        List<Vector3> path = new List<Vector3>();

        for(int i = 0; i < wayPoints.Count; i++)
        {
            bool addPoint = Random.Range(0.0f, 1.0f) <= 0.5 ? false : true;

            if(path.Count <= 0)
            {
                if (addPoint)
                {
                    path.Add(wayPoints[i].position);
                    continue;
                }
            }
            else
            {
                bool pointLowerThanLast = wayPoints[i].position.y <= path[path.Count - 1].y ? true : false;

                if(addPoint && pointLowerThanLast)
                {
                    path.Add(wayPoints[i].position);
                    continue;
                }
            }
        }

        int randomEndPoint = Random.Range(0, endPoints.Count - 1);
        path.Add(endPoints[randomEndPoint].position);

        return path.ToArray();
    }
}
