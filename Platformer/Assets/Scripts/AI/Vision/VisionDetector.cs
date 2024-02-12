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

    public int DetectionCount { get; protected set; }

    public abstract int Detect(Vector2 origin);
    protected abstract void UpdateDetectFunction();
    public abstract void DrawGizmos(Vector2 origin);
}
