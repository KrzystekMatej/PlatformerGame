using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

#if UNITY_EDITOR

public class LineCastCheck : MonoBehaviour
{
    [SerializeField]
    private LayerMask colliderMask;
    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField]
    private bool isTestingActive;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(pointA.position, pointB.position);
        if (isTestingActive)
        {
            Debug.Log(Physics2D.Linecast(pointA.position, pointB.position, colliderMask).collider);
        }
    }
}

#endif
