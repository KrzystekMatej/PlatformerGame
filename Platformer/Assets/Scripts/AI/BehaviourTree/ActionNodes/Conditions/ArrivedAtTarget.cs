using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ArrivedAtTarget : Condition
{
    private float arriveDistance = 1;

    protected override bool IsConditionSatisfied()
    {
        return Vector3.Distance((Vector3)blackboard.DataTable["CurrentTarget"], context.Agent.GetCenterPosition()) < arriveDistance;
    }
}
