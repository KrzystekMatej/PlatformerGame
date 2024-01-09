using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SwitchSteeringPipeline : ActionNode
{
    [SerializeField]
    private string targetSteeringPipeline;

    protected override State OnUpdate()
    {
        context.Steering.CurrentPipeline = targetSteeringPipeline;
        return State.Success;
    }
}