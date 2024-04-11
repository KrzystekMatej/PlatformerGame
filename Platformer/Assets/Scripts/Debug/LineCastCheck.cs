#if UNITY_EDITOR

using UnityEngine;

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
            Vector2 goalVector = pointB.position - pointA.position;
            float goalDistance = goalVector.magnitude;
            Debug.Log(Physics2D.Raycast(pointA.position, goalVector, goalDistance, colliderMask).collider);
        }
    }
}

#endif
