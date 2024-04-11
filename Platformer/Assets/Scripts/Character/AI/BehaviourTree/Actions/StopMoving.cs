using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class StopMoving : ActionNode
{
    [SerializeField]
    bool stopImmediately;

    protected override void OnStart()
    {
        if (!stopImmediately) context.InputController.StopMoving();
        context.Steering.UpdateCurrentPipeline(null);
    }

    protected override ProcessState OnUpdate()
    {
        if (stopImmediately)
        {
            context.Agent.RigidBody.velocity = Vector2.zero;
            return ProcessState.Success;
        }

        return context.Agent.RigidBody.velocity.magnitude > 0.001 ? ProcessState.Running : ProcessState.Success;
    }


    protected override void OnStop() { }
}