using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class GetPathTarget : ActionNode
{
    [SerializeField]
    NodeProperty<Vector2> pathTarget;
    [SerializeField]
    NodeProperty<PathTargeter> pathTargeter;

    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        pathTarget.Value = pathTargeter.Value.GetPathTarget();
        return ProcessState.Success;
    }

    protected override void OnStop() { }
}
