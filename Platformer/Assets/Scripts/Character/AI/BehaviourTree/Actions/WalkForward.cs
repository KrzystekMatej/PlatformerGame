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
        seekTargeter.Value.GoalPosition = context.Agent.TriggerCenter + context.Agent.OrientationController.CurrentOrientation * MathUtility.LongDistance;

        return context.Steering.Recalculate();
    }

    protected override void OnStop() { }
}
