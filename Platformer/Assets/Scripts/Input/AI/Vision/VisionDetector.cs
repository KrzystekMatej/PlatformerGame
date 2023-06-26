using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisionDetector : MonoBehaviour
{
    [Header("Gizmo parameters")]
    [SerializeField]
    protected Color gizmoColor = Color.red;

    public abstract IEnumerator Detect(float delay);
}
