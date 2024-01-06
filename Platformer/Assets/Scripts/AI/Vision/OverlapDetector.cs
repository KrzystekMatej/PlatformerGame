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

    public void Initialize(Color gizmoColor, Vector2 size, float angle, LayerMask detectLayerMask, Vector2 originOffset,
        ShapeType detectShapeType, int maxDetections, LayerMask blockLayerMask)
    {
        Initialize(gizmoColor, size, angle, detectLayerMask, originOffset, detectShapeType, maxDetections);
        this.Colliders = new Collider2D[maxDetections];
        this.BlockLayerMask = blockLayerMask;
    }

    public override int Detect(Vector2 origin)
    {
        origin += OriginOffset;
        int detectionCount = 0;
        switch (DetectShapeType)
        {
            case ShapeType.Box:
                detectionCount = Physics2D.OverlapBoxNonAlloc(origin, Size, Angle, Colliders, DetectLayerMask);
                break;
            case ShapeType.Circle:
                detectionCount = Physics2D.OverlapCircleNonAlloc(origin, Size.x, Colliders, DetectLayerMask);
                break;
        }

        if (BlockLayerMask != 0)
        {
            return FilterBlockedColliders(origin, detectionCount);
        }

        return detectionCount;
    }

    public int FilterBlockedColliders(Vector2 origin, int detectionCount)
    {
        for (int i = 0; i < detectionCount; i++)
        {
            Vector2 directionToTarget = (Vector2)Colliders[i].bounds.center - origin;
            float distance = directionToTarget.magnitude;
            if (distance == 0) break;
            RaycastHit2D hit = Physics2D.Raycast(origin, directionToTarget/distance, Size.x, BlockLayerMask | DetectLayerMask);

            if (hit.collider == null || !Utility.CheckLayer(hit.collider.gameObject.layer, DetectLayerMask))
            {
                Colliders[i] = null;
                detectionCount--;
            }
        }

        return detectionCount;
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
