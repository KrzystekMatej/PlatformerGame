using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ReactivatePathFollowing : ActionNode
{
    protected override State OnUpdate()
    {
        PathFollowingTargeter pathFollowing = (PathFollowingTargeter)blackboard.DataTable[context.Steering.CurrentPipelineName + nameof(PathFollowingTargeter)];
        pathFollowing.RecalculatePath(context.Agent);
        return State.Success;
    }
}

