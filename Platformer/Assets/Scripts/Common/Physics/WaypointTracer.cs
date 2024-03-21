using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WaypointTracer : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private Transform[] waypoints;
#endif
    [field: SerializeField]
    public bool isCircular { get; set; } = true;
    [SerializeField] 
    private float speed = 2f;
    public UnityEvent OnPathFinished;


    [field: SerializeField]
    [field: HideInInspector]
    public List<Vector3> PathPoints { get; private set; } = new List<Vector3>();
    private int current = 0;

    private void OnValidate()
    {
        PathPoints.Clear();
        PathPoints.AddRange(waypoints.Select(w => w.position).ToList());
    }

    private void Update()
    {
        if (transform.position == PathPoints[current])
        {
            current++;
            if (isCircular) current = MathUtility.GetCircularIndex(current, PathPoints.Count);
            else if (current >= PathPoints.Count)
            {
                enabled = false;
                OnPathFinished?.Invoke();
                return;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, PathPoints[current], Time.deltaTime * speed);
    }
}
