using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisionDetector : MonoBehaviour
{
    [field: SerializeField]
    public string DetectorName { get; protected set; }
    [Header("Gizmo parameters")]
    [SerializeField]
    protected Color gizmoColor = Color.red;

    public abstract void Detect();
}
