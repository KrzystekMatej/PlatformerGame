using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ChangeMaxSpeed : ActionNode
{
    [SerializeField]
    private float maxSpeedMultiplier = 2f;

    protected override void OnStart() { }

    protected override NodeState OnUpdate()
    {
        context.Agent.InstanceData.MaxSpeed = context.Agent.DefaultData.MaxSpeed * maxSpeedMultiplier;
        return NodeState.Success;
    }

    protected override void OnStop() { }
}
