using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SwitchSteeringPipeline : ActionNode
{
    [SerializeField]
    private NodeProperty<SteeringPipeline> targetPipeline;

    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        context.Steering.UpdateCurrentPipeline(targetPipeline.Value);
        return ProcessState.Success;
    }

    protected override void OnStop() { }
}