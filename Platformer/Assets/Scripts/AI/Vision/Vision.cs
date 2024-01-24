using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
public class Vision : MonoBehaviour
{
    [SerializeField]
    private List<VisionDetector> detectors = new List<VisionDetector>();

    public void AddCastDetector(Color gizmoColor, Vector2 size, float angle, LayerMask detectLayerMask, Vector2 originOffset,
        ShapeType detectShapeType, int maxDetections, float distance, Vector2 direction)
    {
        CastDetector detector = ScriptableObject.CreateInstance<CastDetector>();
        detector.Initialize(gizmoColor, size, angle, detectLayerMask, originOffset,
        detectShapeType, maxDetections, distance, direction);
        detectors.Add(detector);
    }

    public void AddAreaDetector(Color gizmoColor, Vector2 size, float angle, LayerMask detectLayerMask, Vector2 originOffset,
        ShapeType detectShapeType, int maxDetections, LayerMask blockLayerMask)
    {
        OverlapDetector detector = ScriptableObject.CreateInstance<OverlapDetector>();
        detector.Initialize(gizmoColor, size, angle, detectLayerMask, originOffset, detectShapeType, maxDetections, blockLayerMask);
        detectors.Add(detector);
    }

    public void OnDrawGizmosSelected()
    {
        Agent agent = GetComponentInParent<AIInputController>().GetComponentInChildren<Agent>();
        Vector2 origin = agent.GetComponent<Collider2D>().bounds.center;

        foreach (VisionDetector detector in detectors)
        {
            detector.DrawGizmos(origin);
        }
    }
}
#endif