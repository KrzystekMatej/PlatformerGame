using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OverlapDetector", menuName = "Vision/OverlapDetector")]
public class OverlapDetector : VisionDetector
{
    public Collider2D[] Colliders { get; private set; }

    private Func<Vector2, Vector2, CapsuleDirection2D, float, Collider2D[], LayerMask, int> overlap;

    private static readonly Func<Vector2, Vector2, CapsuleDirection2D, float, Collider2D[], LayerMask, int> pointOverlap
        = (origin, size, capsuleDirection, angle, colliders, layerMask)
        => Physics2D.OverlapPointNonAlloc(origin, colliders, layerMask);
    private static readonly Func<Vector2, Vector2, CapsuleDirection2D, float, Collider2D[], LayerMask, int> boxOverlap
        = (origin, size, capsuleDirection, angle, colliders, layerMask)
        => Physics2D.OverlapBoxNonAlloc(origin, size, angle, colliders, layerMask);
    private static readonly Func<Vector2, Vector2, CapsuleDirection2D, float, Collider2D[], LayerMask, int> circleOverlap
        = (origin, size, capsuleDirection, angle, colliders, layerMask)
        => Physics2D.OverlapCircleNonAlloc(origin, size.x, colliders, layerMask);
    private static readonly Func<Vector2, Vector2, CapsuleDirection2D, float, Collider2D[], LayerMask, int> capsuleOverlap
        = (origin, size, capsuleDirection, angle, colliders, layerMask)
        => Physics2D.OverlapCapsuleNonAlloc(origin, size, capsuleDirection, angle, colliders, layerMask);


    private void OnEnable()
    {
        Colliders = new Collider2D[maxDetections];

        UpdateDetectFunction();
    }

    protected override void UpdateDetectFunction()
    {
        switch (DetectShapeType)
        {
            case ShapeType.Primitive:
                overlap = pointOverlap;
                break;
            case ShapeType.Box:
                overlap = boxOverlap;
                break;
            case ShapeType.Circle:
                overlap = circleOverlap;
                break;
            case ShapeType.Capsule:
                overlap = capsuleOverlap;
                break;
        }
    }

    public override int Detect(Vector2 origin)
        => overlap(origin + OriginOffset, Size, CapsuleDirection, Angle, Colliders, DetectLayerMask);

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
