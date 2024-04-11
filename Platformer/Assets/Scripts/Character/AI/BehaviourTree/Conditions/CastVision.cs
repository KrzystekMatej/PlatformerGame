using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CastVision : ConditionNode
{
    [SerializeField]
    private NodeProperty<List<RaycastHit2D>> hits;
    [SerializeField]
    private CastDetector visionDetector;
    [SerializeField]
    private float distance;
    [SerializeField]
    private bool changeDirectionByOrientation;
    [SerializeField]
    private bool changeOffsetByOrientation;
#if UNITY_EDITOR
    [SerializeField]
    private Color gizmoColor = Color.red;
#endif

    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        Vector2 offset = visionDetector.OriginOffset;
        InitializeDetector(context.Agent.OrientationController.CurrentOrientation);
        int detectionCount = visionDetector.Detect(context.Agent.CenterPosition);

        if (hits.IsBlackboardKey())
        {
            hits.Value.Clear();
            for (int i = 0; i < detectionCount; i++) hits.Value.Add(visionDetector.Hits[i]);
        }

        visionDetector.OriginOffset = offset;
        return detectionCount > 0;
    }

    protected override void OnStop() { }

    private void InitializeDetector(Vector2 orientation)
    {
        visionDetector.Distance = distance;
        if (changeDirectionByOrientation)
        {
            visionDetector.Direction = orientation;
        }
        if (changeOffsetByOrientation)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.right, orientation));
            visionDetector.OriginOffset = rotation * visionDetector.OriginOffset;
        }
    }

#if UNITY_EDITOR

    public override void OnDrawGizmos(AgentManager agent)
    {
        Vector2 offset = visionDetector.OriginOffset;
        OrientationController orientation = agent.GetComponentInChildren<OrientationController>();
        InitializeDetector(orientation.CurrentOrientation);
        visionDetector.GizmoColor = gizmoColor;
        visionDetector.DrawGizmos(agent.GetComponent<Collider2D>().bounds.center);
        visionDetector.OriginOffset = offset;
    }
#endif
}
