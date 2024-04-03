using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class WalkForward : ActionNode
{
    private SeekTargeter seekTargeter;
    private Vector2 turnedDirection;

    protected override void OnStart()
    {
        seekTargeter = blackboard.GetValue<SeekTargeter>(context.Steering.CurrentPipelineName + nameof(SeekTargeter));
    }

    protected override ProcessState OnUpdate()
    {
        const float lookAhead = 2f;
        seekTargeter.GoalPosition = context.Agent.CenterPosition + context.Agent.OrientationController.CurrentOrientation * context.Agent.EnclosingCircleRadius * lookAhead;
        return ProcessState.Running;
    }

    protected override void OnStop() { }
}
