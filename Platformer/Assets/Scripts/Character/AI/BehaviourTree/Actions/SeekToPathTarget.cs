using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SeekToPathTarget : ActionNode
{
    [SerializeField]
    NodeProperty<PathTargeter> pathTargeter;
    [SerializeField]
    NodeProperty<SeekTargeter> seekTargeter;

    protected override void OnStart()
    {
        seekTargeter.Value.MaxSeekDistance = float.PositiveInfinity;
        seekTargeter.Value.GoalPosition = pathTargeter.Value.GetPathTarget();
        seekTargeter.Value.GoalOwner = null;
    }

    protected override ProcessState OnUpdate()
    {
        return context.Steering.Recalculate();
    }

    protected override void OnStop() { }
}
