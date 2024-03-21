using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SwitchSteeringPipeline : ActionNode
{
    [SerializeField]
    private string targetPipelineName;

    private SteeringPipeline targetPipeline;

    public override void OnInit()
    {
        targetPipeline = context.Steering.GetPipeline(targetPipelineName);
    }

    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        bool hasChanged = context.Steering.UpdateCurrentPipeline(targetPipeline);
        return hasChanged ? ProcessState.Success : ProcessState.Failure;
    }

    protected override void OnStop() { }
}