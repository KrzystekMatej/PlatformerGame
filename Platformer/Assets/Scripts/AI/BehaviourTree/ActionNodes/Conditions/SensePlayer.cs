using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class SensePlayer : Condition
{
    [SerializeField]
    private OverlapDetector senseDetector;


    public override void Initialize()
    {
        blackboard.DataTable["GoalPosition"] = null;
        blackboard.DataTable["TargetOwner"] = null;
    }

    protected override bool IsConditionSatisfied()
    {
        int detectionCount = senseDetector.Detect(context.Agent.CenterPosition);
        if (detectionCount > 0)
        {
            blackboard.DataTable["GoalPosition"] = (Vector2)senseDetector.Colliders[0].gameObject.transform.position;
            blackboard.DataTable["GoalOwner"] = senseDetector.Colliders[0].gameObject;
            return true;
        }
        blackboard.DataTable["GoalPosition"] = null;
        blackboard.DataTable["GoalOwner"] = null;
        return false;
    }
}
