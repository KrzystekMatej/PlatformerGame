using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CastDetector", menuName = "Vision/CastDetector")]
public class CastDetector : VisionDetector
{
    [field: SerializeField]
    public float Distance { get; set; }
    [field: SerializeField]
    public Vector2 Direction { get; set; }
    public RaycastHit2D[] Hits { get; private set; }

    private void OnEnable()
    {
        Hits = new RaycastHit2D[maxDetections];
    }


    public void Initialize(Color gizmoColor, Vector2 size, float angle, LayerMask detectLayerMask, Vector2 originOffset,
        ShapeType detectShapeType, int maxDetections, float distance, Vector2 direction)
    {
        Initialize(gizmoColor, size, angle, detectLayerMask, originOffset, detectShapeType, maxDetections);
        this.Hits = new RaycastHit2D[maxDetections];
        this.Distance = distance;
        this.Direction = direction.normalized;
    }

    public override int Detect(Vector2 origin)
    {
        origin += OriginOffset;
        switch (DetectShapeType)
        {
            case ShapeType.Ray:
                return Physics2D.RaycastNonAlloc(origin, Direction, Hits, Distance, DetectLayerMask);
            case ShapeType.Box:
                return Physics2D.BoxCastNonAlloc(origin, Size, Angle, Direction, Hits, Distance, DetectLayerMask);
            case ShapeType.Circle:
                return Physics2D.CircleCastNonAlloc(origin, Size.x, Direction, Hits, Distance, DetectLayerMask);
            default:
                return 0;
        }
    }

    public override void DrawGizmos(Vector2 origin)
    {
        Gizmos.color = gizmoColor;
        origin += OriginOffset;
        switch (DetectShapeType)
        {
            case ShapeType.Ray:
                Vector2 end = origin + Direction * Distance;
                Gizmos.DrawLine(origin, end);
                return;
            case ShapeType.Box:
                Gizmos.DrawWireCube(origin, Size);
                return;
            case ShapeType.Circle:
                Gizmos.DrawWireSphere(origin, Size.x);
                return;
        }
    }
}