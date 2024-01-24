using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SeekToTarget : ActionNode
{
    protected override State OnUpdate()
    {
        SeekTargeter seek = (SeekTargeter)blackboard.DataTable[context.Steering.CurrentPipelineName + nameof(SeekTargeter)];
        seek.TargetPosition = (Vector2)blackboard.DataTable["Target"];
        return State.Success;
    }
}
