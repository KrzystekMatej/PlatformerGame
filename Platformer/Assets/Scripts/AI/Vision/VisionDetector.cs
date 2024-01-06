using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisionDetector : ScriptableObject
{
    [SerializeField]
    protected Color gizmoColor = Color.red;
    [field: SerializeField]
    public Vector2 Size { get; set; }
    [field: SerializeField]
    public float Angle { get; set; }
    [field: SerializeField]
    public LayerMask DetectLayerMask { get; set; }
    [field: SerializeField]
    public Vector2 OriginOffset { get; set; }
    [field: SerializeField]
    public ShapeType DetectShapeType { get; protected set; }
    [SerializeField]
    protected int maxDetections = 1;

    public void Initialize(Color gizmoColor, Vector2 size, float angle, LayerMask detectLayerMask, Vector2 originOffset, ShapeType detectShapeType, int maxDetections)
    {
        this.gizmoColor = gizmoColor;
        this.Size = size;
        this.Angle = angle;
        this.DetectLayerMask = detectLayerMask;
        this.OriginOffset = originOffset;
        this.DetectShapeType = detectShapeType;
        this.maxDetections = maxDetections;
    }

    public abstract int Detect(Vector2 origin);

    public abstract void DrawGizmos(Vector2 origin);
}
