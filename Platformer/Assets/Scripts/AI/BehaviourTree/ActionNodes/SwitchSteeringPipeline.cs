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
        bool hasChanged = context.Steering.UpdateCurrentPipeline(targetSteeringPipeline);
        return hasChanged ? State.Success : State.Failure;
    }
}