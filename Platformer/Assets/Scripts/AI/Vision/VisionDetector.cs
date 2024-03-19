using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class VisionDetector : ScriptableObject
{
    [field: SerializeField]
    public Color GizmoColor { get; set; } = Color.red;
    [field: SerializeField]
    public Vector2 Size { get; set; }
    [field: SerializeField]
    public CapsuleDirection2D CapsuleDirection { get; set; }
    [field: SerializeField]
    public float Angle { get; set; }
    [field: SerializeField]
    public LayerMask DetectLayerMask { get; set; }
    [field: SerializeField]
    public Vector2 OriginOffset { get; set; }

    [SerializeField]
    private ShapeType detectShapeType;
    public ShapeType DetectShapeType
    {
        get => detectShapeType;
        set
        {
            detectShapeType = value;
            UpdateDetectFunction();
        }
    }

    [SerializeField]
    protected int maxDetections = 1;

    public abstract int Detect(Vector2 origin);
    protected abstract void UpdateDetectFunction();

#if UNITY_EDITOR
    public abstract void DrawGizmos(Vector2 origin);

    protected void DrawBox(Vector2 origin, (Vector2 rightUp, Vector2 rightDown, Vector2 leftUp, Vector2 leftDown) box)
    {
        Gizmos.DrawLine(origin + box.rightUp, origin + box.rightDown);
        Gizmos.DrawLine(origin + box.rightUp, origin + box.leftUp);
        Gizmos.DrawLine(origin + box.rightDown, origin + box.leftDown);
        Gizmos.DrawLine(origin + box.leftUp, origin + box.leftDown);
    }

    protected (Vector2 rightUp, Vector2 rightDown, Vector2 leftUp, Vector2 leftDown) GetRotatedBox()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Angle);
        Vector2 halfSize = Size / 2;
        Vector2 rightUp = rotation * halfSize;
        Vector2 rightDown = rotation * new Vector2(halfSize.x, -halfSize.y);
        Vector2 leftUp = rotation * new Vector2(-halfSize.x, halfSize.y);
        Vector2 leftDown = rotation  * -halfSize;
        return (rightUp, rightDown, leftUp, leftDown);
    }
#endif
}
