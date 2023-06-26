using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : AreaDetector
{

    public override IEnumerator Detect(float delay)
    {
        while (true)
        {
            ColliderCount = Physics2D.OverlapCircleNonAlloc(transform.position, DetectionRadius, colliders, detectLayerMask);
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = gizmoColor;
            for (int i = 0; i < ColliderCount; i++)
            {
                Gizmos.DrawSphere(colliders[i].bounds.center, 0.2f);
            }
        }
    }
}
