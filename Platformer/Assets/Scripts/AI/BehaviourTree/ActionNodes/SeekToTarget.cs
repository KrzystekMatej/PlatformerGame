using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SeekToTarget : ActionNode
{
    protected override void OnStart() { }

    protected override NodeState OnUpdate()
    {
        SeekTargeter seek = blackboard.GetValue<SeekTargeter>(context.Steering.CurrentPipelineName + nameof(SeekTargeter));
        seek.GoalPosition = blackboard.GetValue<Vector2>("GoalPosition");
        seek.GoalOwner = blackboard.GetValue<GameObject>("GoalOwner");
        return NodeState.Success;
    }

    protected override void OnStop() { }
}
