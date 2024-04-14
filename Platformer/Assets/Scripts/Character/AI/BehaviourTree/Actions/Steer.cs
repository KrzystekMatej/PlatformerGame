using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class Steer : ActionNode
{
    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        return context.Steering.Recalculate();
    }

    protected override void OnStop() { }
}
