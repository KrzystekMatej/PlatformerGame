using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SteeringListener : ActionNode
{
    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        return context.Steering.State;
    }

    protected override void OnStop() { }
}
