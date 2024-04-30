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

    private Vector2 pathTarget;

    protected override void OnStart()
    {
        pathTarget = pathTargeter.Value.GetPathTarget();
    }

    protected override ProcessState OnUpdate()
    {
        seekTargeter.Value.MaxSeekDistance = float.PositiveInfinity;
        seekTargeter.Value.GoalPosition = pathTarget;
        seekTargeter.Value.GoalOwner = null;
        return context.Steering.Recalculate();
    }

    protected override void OnStop() { }
}
