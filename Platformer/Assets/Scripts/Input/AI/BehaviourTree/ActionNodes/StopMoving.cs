using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class StopMoving : ActionNode
{
    protected override void OnStart()
    {
        if (context.Steering != null) context.Steering.UpdateCurrentPipeline(null);
        context.InputController.StopMoving();
        position = context.Agent.CenterPosition;
    }

    protected override ProcessState OnUpdate()
    {
        return context.Agent.RigidBody.velocity.magnitude > 0.001 ? ProcessState.Running : ProcessState.Success;
    }


    protected override void OnStop() { }
}