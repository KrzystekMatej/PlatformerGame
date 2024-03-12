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
    }

    protected override NodeState OnUpdate()
    {
        return context.Agent.RigidBody.velocity.magnitude > 0.01 ? NodeState.Running : NodeState.Success;
    }

    protected override void OnStop() { }
}
