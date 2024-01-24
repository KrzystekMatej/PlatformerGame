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
        blackboard.DataTable["Target"] = null;
    }

    protected override bool IsConditionSatisfied()
    {
        int detectionCount = senseDetector.Detect(context.Agent.CenterPosition);
        if (detectionCount > 0)
        {
            blackboard.DataTable["Target"] = (Vector2)senseDetector.Colliders[0].bounds.center;
            return true;
        }
        blackboard.DataTable["Target"] = null;
        return false;
    }
}
