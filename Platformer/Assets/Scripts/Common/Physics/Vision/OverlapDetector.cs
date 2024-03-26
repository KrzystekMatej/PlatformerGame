using System;
using Unity.VisualScripting;
using UnityEngine;
using Overlap = System.Func<UnityEngine.Vector2, UnityEngine.Vector2, UnityEngine.CapsuleDirection2D,
    float, UnityEngine.Collider2D[], UnityEngine.LayerMask, int>;

[CreateAssetMenu(fileName = "OverlapDetector", menuName = "Vision/OverlapDetector")]
public class OverlapDetector : VisionDetector
{
    public Collider2D[] Colliders { get; private set; }

    private Overlap overlap;

    private static readonly Overlap pointOverlap = (origin, size, capsuleDirection, angle, colliders, layerMask)
        => Physics2D.OverlapPointNonAlloc(origin, colliders, layerMask);
    private static readonly Overlap boxOverlap = (origin, size, capsuleDirection, angle, colliders, layerMask)
        => Physics2D.OverlapBoxNonAlloc(origin, size, angle, colliders, layerMask);
    private static readonly Overlap circleOverlap = (origin, size, capsuleDirection, angle, colliders, layerMask)
        => Physics2D.OverlapCircleNonAlloc(origin, size.x, colliders, layerMask);
    private static readonly Overlap capsuleOverlap = (origin, size, capsuleDirection, angle, colliders, layerMask)
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

#if UNITY_EDITOR
    public override void DrawGizmos(Vector2 origin)
    {
        Gizmos.color = GizmoColor;

        origin += OriginOffset;

        switch (DetectShapeType)
        {
            case ShapeType.Primitive:
                Gizmos.DrawWireSphere(origin, 0.1f);
                return;
            case ShapeType.Box:
                DrawBox(origin, GetRotatedBox());
                return;
            case ShapeType.Circle:
                Gizmos.DrawWireSphere(origin, Size.x);
                return;
        }
    }
#endif
}
