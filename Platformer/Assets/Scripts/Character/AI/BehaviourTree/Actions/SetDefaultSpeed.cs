using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetDefaultSpeed : ActionNode
{
    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        context.Agent.InstanceData.MaxSpeed = context.Agent.DefaultData.MaxSpeed;
        return ProcessState.Success;
    }

    protected override void OnStop() { }
}
