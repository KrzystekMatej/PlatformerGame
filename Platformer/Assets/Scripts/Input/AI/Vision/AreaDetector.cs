using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDetector : VisionDetector
{
    [field: SerializeField]
    public float DetectionRadius { get; protected set; }
    [SerializeField]
    private int maxCollidersToDetect;
    [SerializeField]
    private LayerMask detectLayerMask;
    [SerializeField]
    private LayerMask blockLayerMask;
    private Collider2D[] colliders;
    public int ColliderCount { get; protected set; }

    private void Awake()
    {
        colliders = new Collider2D[maxCollidersToDetect];
    }

    public void Initialize(string detectorName, Color gizmoColor, float detectionRadius, int maxCollidersToDetect, LayerMask detectLayerMask, LayerMask blockLayerMask)
    {
        this.DetectorName = detectorName;
        this.gizmoColor = gizmoColor;
        this.DetectionRadius = detectionRadius;
        this.maxCollidersToDetect = maxCollidersToDetect;
        this.detectLayerMask = detectLayerMask;
        this.blockLayerMask = blockLayerMask;
    }

    public Collider2D[] GetColliders()
    {
        return colliders;
    }

    public override void Detect()
    {
        ColliderCount = Physics2D.OverlapCircleNonAlloc(transform.position, DetectionRadius, colliders, detectLayerMask);

        if (blockLayerMask != 0)
        {
            FilterBlockedColliders();
        }
    }

    public void FilterBlockedColliders()
    {
        for (int i = 0; i < ColliderCount; i++)
        {
            Vector2 directionToTarget = (colliders[i].bounds.center - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, DetectionRadius, blockLayerMask | detectLayerMask);

            if (hit.collider == null || !Utility.CheckLayer(hit.collider.gameObject.layer, detectLayerMask))
            {
                colliders[i] = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireSphere(transform.position, DetectionRadius);


        if (Application.isPlaying)
        {
            for (int i = 0; i < ColliderCount; i++)
            {
                if (colliders[i] != null)
                {
                    Gizmos.DrawSphere(colliders[i].bounds.center, 0.3f);
                }
            }
        }
    }
}
