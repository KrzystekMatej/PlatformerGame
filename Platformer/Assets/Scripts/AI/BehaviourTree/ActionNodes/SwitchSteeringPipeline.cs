using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SwitchSteeringPipeline : ActionNode
{
    [SerializeField]
    private string targetPipelineName;

    private SteeringPipeline targetPipeline;

    public override void Initialize()
    {
        targetPipeline = context.Steering.GetPipeline(targetPipelineName);
    }

    protected override State OnUpdate()
    {
        bool hasChanged = context.Steering.UpdateCurrentPipeline(targetPipeline);
        return hasChanged ? State.Success : State.Failure;
    }
}