using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class SensePlayer : Condition
{
    [SerializeField]
    private OverlapDetector senseDetector;


    public override void OnInit()
    {
        blackboard.SetValue<Vector2?>("GoalPosition", null);
        blackboard.SetValue<GameObject>("TargetOwner", null);
    }

    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        int detectionCount = senseDetector.Detect(context.Agent.CenterPosition);
        if (detectionCount > 0)
        {
            blackboard.SetValue<Vector2?>("GoalPosition", senseDetector.Colliders[0].gameObject.transform.position);
            blackboard.SetValue("TargetOwner", senseDetector.Colliders[0].gameObject);
            return true;
        }
        blackboard.SetValue<Vector2?>("GoalPosition", null);
        blackboard.SetValue<GameObject>("TargetOwner", null);
        return false;
    }


    protected override void OnStop() { }
}
