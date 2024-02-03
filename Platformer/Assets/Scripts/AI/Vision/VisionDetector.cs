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

    public int DetectionCount { get; protected set; }

    public abstract int Detect(Vector2 origin);

    public abstract void DrawGizmos(Vector2 origin);
}
