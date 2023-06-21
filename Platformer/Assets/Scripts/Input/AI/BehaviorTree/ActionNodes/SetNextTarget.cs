using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetNextTarget : ActionNode
{
    private Vector3[] targets;
    private int current = 0;

    public override void Initialize()
    {
        targets = (Vector3[])blackboard.DataTable["Targets"];
        blackboard.DataTable["CurrentTarget"] = targets[current];
    }

    protected override State OnUpdate()
    {
        current = (current + 1) % targets.Length;
        blackboard.DataTable["CurrentTarget"] = targets[current];
        return State.Success;
    }
}
