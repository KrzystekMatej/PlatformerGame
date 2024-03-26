using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CastVision : Condition
{
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

    protected override void OnStart()
    {
        InitializeDetector(context.Agent.OrientationController.CurrentOrientation);
    }

    protected override bool IsConditionSatisfied()
    {
        int detectionCount = visionDetector.Detect(context.Agent.CenterPosition);
        blackboard.SetValue(visionDetector.name + "VisionHits", visionDetector.Hits);
        blackboard.SetValue(visionDetector.name + "VisionCount", detectionCount);
        return detectionCount > 0;
    }

    protected override void OnStop() { }

    private void InitializeDetector(float orientation)
    {
        visionDetector.Distance = distance;
        if (changeDirectionByOrientation)
        {
            visionDetector.Direction = new Vector2(orientation, 0);
        }
        if (changeOffsetByOrientation)
        {
            float xOffset = Mathf.Abs(visionDetector.OriginOffset.x) * orientation;
            visionDetector.OriginOffset = new Vector2(xOffset, visionDetector.OriginOffset.y);
        }
    }

#if UNITY_EDITOR

    public override void OnDrawGizmos(AgentManager agent)
    {
        InitializeDetector(agent.GetComponentInChildren<OrientationController>().CurrentOrientation);
        visionDetector.GizmoColor = gizmoColor;
        visionDetector.DrawGizmos(agent.GetComponent<Collider2D>().bounds.center);   
    }
#endif
}
