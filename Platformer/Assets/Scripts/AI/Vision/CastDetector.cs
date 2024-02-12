using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "CastDetector", menuName = "Vision/CastDetector")]
public class CastDetector : VisionDetector
{
    [field: SerializeField]
    public float Distance { get; set; }
    [field: SerializeField]
    public Vector2 Direction { get; set; }
    public RaycastHit2D[] Hits { get; private set; }
    private Func<Vector2, Vector2, CapsuleDirection2D, float, Vector2, RaycastHit2D[], float, LayerMask, int> cast;

    private static readonly Func<Vector2, Vector2, CapsuleDirection2D, float, Vector2, RaycastHit2D[], float, LayerMask, int> rayCast
        = (origin, size, capsuleDirection, angle, direction, hits, distance, layerMask)
        => Physics2D.RaycastNonAlloc(origin, direction, hits, distance, layerMask);
    private static readonly Func<Vector2, Vector2, CapsuleDirection2D, float, Vector2, RaycastHit2D[], float, LayerMask, int> boxCast
        = (origin, size, capsuleDirection, angle, direction, hits, distance, layerMask)
        => Physics2D.BoxCastNonAlloc(origin, size, angle, direction, hits, distance, layerMask);
    private static readonly Func<Vector2, Vector2, CapsuleDirection2D, float, Vector2, RaycastHit2D[], float, LayerMask, int> circleCast
        = (origin, size, capsuleDirection, angle, direction, hits, distance, layerMask)
        => Physics2D.CircleCastNonAlloc(origin, size.x, direction, hits, distance, layerMask);
    private static readonly Func<Vector2, Vector2, CapsuleDirection2D, float, Vector2, RaycastHit2D[], float, LayerMask, int> capsuleCast
        = (origin, size, capsuleDirection, angle, direction, hits, distance, layerMask)
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
}