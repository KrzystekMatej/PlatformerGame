using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ReactivatePathFollowing : ActionNode
{
    protected override void OnStart() { }

    protected override NodeState OnUpdate()
    {
        PathFollowingTargeter pathFollowing = blackboard.GetValue<PathFollowingTargeter>(context.Steering.CurrentPipelineName + nameof(PathFollowingTargeter));
        pathFollowing.RecalculatePath(context.Agent);
        return NodeState.Success;
    }

    protected override void OnStop() { }
}

