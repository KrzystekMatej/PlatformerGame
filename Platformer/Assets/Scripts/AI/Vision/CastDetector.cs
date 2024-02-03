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
        Direction = Direction.normalized;
    }

    public override int Detect(Vector2 origin)
    {
        origin += OriginOffset;
        switch (DetectShapeType)
        {
            case ShapeType.Ray:
                DetectionCount = Physics2D.RaycastNonAlloc(origin, Direction, Hits, Distance, DetectLayerMask);
                break;
            case ShapeType.Box:
                DetectionCount = Physics2D.BoxCastNonAlloc(origin, Size, Angle, Direction, Hits, Distance, DetectLayerMask);
                break;
            case ShapeType.Circle:
                DetectionCount = Physics2D.CircleCastNonAlloc(origin, Size.x, Direction, Hits, Distance, DetectLayerMask);
                break;
            default:
                DetectionCount = 0;
                break;
        }
        return DetectionCount;
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