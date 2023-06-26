using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TargetDetector : AreaDetector
{
    [SerializeField]
    private LayerMask obstacleLayerMask;

    public override IEnumerator Detect(float delay)
    {
        while (true)
        {
            ColliderCount = Physics2D.OverlapCircleNonAlloc(transform.position, DetectionRadius, colliders, detectLayerMask);

            for (int i = 0; i < ColliderCount; i++) 
            {
                Vector2 directionToTarget = (colliders[i].bounds.center - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, DetectionRadius, obstacleLayerMask | detectLayerMask);

                if (hit.collider == null || !Utility.CheckLayer(hit.collider.gameObject.layer, detectLayerMask))
                {
                    colliders[i] = null;
                }
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);

        Gizmos.color = gizmoColor;
        for (int i = 0; i < ColliderCount; i++)
        {
            if (colliders[i] != null)
            {
                Gizmos.DrawSphere(colliders[i].bounds.center, 0.3f);
            }
        }
    }
}
