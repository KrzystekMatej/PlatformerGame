using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;
using Cast = System.Func<UnityEngine.Vector2, UnityEngine.Vector2, UnityEngine.CapsuleDirection2D,
    float, UnityEngine.Vector2, UnityEngine.RaycastHit2D[], float, UnityEngine.LayerMask, int>;

[CreateAssetMenu(fileName = "CastDetector", menuName = "Vision/CastDetector")]
public class CastDetector : VisionDetector
{
    [field: SerializeField]
    public float Distance { get; set; }
    [field: SerializeField]
    public Vector2 Direction { get; set; }
    public RaycastHit2D[] Hits { get; private set; }
    private Cast cast;

    private static readonly Cast rayCast = (origin, size, capsuleDirection, angle, direction, hits, distance, layerMask)
        => Physics2D.RaycastNonAlloc(origin, direction, hits, distance, layerMask);
    private static readonly Cast boxCast = (origin, size, capsuleDirection, angle, direction, hits, distance, layerMask)
        => Physics2D.BoxCastNonAlloc(origin, size, angle, direction, hits, distance, layerMask);
    private static readonly Cast circleCast = (origin, size, capsuleDirection, angle, direction, hits, distance, layerMask)
        => Physics2D.CircleCastNonAlloc(origin, size.x, direction, hits, distance, layerMask);
    private static readonly Cast capsuleCast = (origin, size, capsuleDirection, angle, direction, hits, distance, layerMask)
        => Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, hits, distance, layerMask);

    private void OnEnable()
    {
        Hits = new RaycastHit2D[maxDetections];
        Direction = Direction.normalized;
        UpdateDetectFunction();
    }

    public override int Detect(Vector2 origin)
        => cast(origin + OriginOffset, Size, CapsuleDirection, Angle, Direction, Hits, Distance, DetectLayerMask);

    protected override void UpdateDetectFunction()
    {
        switch (DetectShapeType)
        {
            case ShapeType.Primitive:
                cast = rayCast;
                break;
            case ShapeType.Box:
                cast = boxCast;
                break;
            case ShapeType.Circle:
                cast = circleCast;
                break;
            case ShapeType.Capsule:
                cast = capsuleCast;
                break;
        }
    }

#if UNITY_EDITOR
    public override void DrawGizmos(Vector2 origin)
    {
        Gizmos.color = gizmoColor;
        origin += OriginOffset;
        switch (DetectShapeType)
        {
            case ShapeType.Primitive:
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
#endif
}