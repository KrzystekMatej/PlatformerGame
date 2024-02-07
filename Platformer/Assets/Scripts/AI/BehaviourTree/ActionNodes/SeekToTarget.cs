using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SeekToTarget : ActionNode
{
    protected override State OnUpdate()
    {
        SeekTargeter seek = (SeekTargeter)blackboard.DataTable[context.Steering.CurrentPipelineName + nameof(SeekTargeter)];
        seek.GoalPosition = (Vector2)blackboard.DataTable["GoalPosition"];
        seek.GoalOwner = (GameObject)blackboard.DataTable["GoalOwner"];
        return State.Success;
    }
}
