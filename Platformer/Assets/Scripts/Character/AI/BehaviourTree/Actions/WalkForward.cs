using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class WalkForward : ActionNode
{
    [SerializeField]
    private NodeProperty<SeekTargeter> seekTargeter;


    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        const float lookAhead = 2f;
        seekTargeter.Value.GoalPosition = context.Agent.CenterPosition + context.Agent.OrientationController.CurrentOrientation * context.Agent.EnclosingCircleRadius * lookAhead;

        return context.Steering.Recalculate();
    }

    protected override void OnStop() { }
}
