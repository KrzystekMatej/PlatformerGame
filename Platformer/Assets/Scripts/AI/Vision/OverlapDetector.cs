using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "OverlapDetector", menuName = "Vision/OverlapDetector")]
public class OverlapDetector : VisionDetector
{
    [SerializeField]
    public LayerMask BlockLayerMask { get; set; }
    public Collider2D[] Colliders { get; private set; }

    private void OnEnable()
    {
        Colliders = new Collider2D[maxDetections];
    }

    public override int Detect(Vector2 origin)
    {
        origin += OriginOffset;
        DetectionCount = 0;
        switch (DetectShapeType)
        {
            case ShapeType.Box:
                DetectionCount = Physics2D.OverlapBoxNonAlloc(origin, Size, Angle, Colliders, DetectLayerMask);
                break;
            case ShapeType.Circle:
                DetectionCount = Physics2D.OverlapCircleNonAlloc(origin, Size.x, Colliders, DetectLayerMask);
                break;
        }

        if (BlockLayerMask != 0)
        {
            return FilterBlockedColliders(origin);
        }

        return DetectionCount;
    }

    public int FilterBlockedColliders(Vector2 origin)
    {
        for (int i = 0; i < DetectionCount; i++)
        {
            Vector2 directionToTarget = (Vector2)Colliders[i].bounds.center - origin;
            float distance = directionToTarget.magnitude;
            if (distance == 0) break;
            RaycastHit2D hit = Physics2D.Raycast(origin, directionToTarget/distance, Size.x, BlockLayerMask | DetectLayerMask);

            if (hit.collider == null || !Utility.CheckLayer(hit.collider.gameObject.layer, DetectLayerMask))
            {
                Colliders[i] = null;
                DetectionCount--;
            }
        }

        return DetectionCount;
    }

    public override void DrawGizmos(Vector2 origin)
    {
        Gizmos.color = gizmoColor;
        origin += OriginOffset;

        switch (DetectShapeType)
        {
            case ShapeType.Box:
                Gizmos.DrawWireCube(origin, Size);
                return;
            case ShapeType.Circle:
                Gizmos.DrawWireSphere(origin, Size.x);
                return;
        }
    }
}
