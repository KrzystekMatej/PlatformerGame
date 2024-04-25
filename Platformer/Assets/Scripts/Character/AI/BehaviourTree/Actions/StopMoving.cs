using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class StopMoving : ActionNode
{
    [SerializeField]
    bool stopImmediately;

    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        context.Steering.Restart();
        context.InputController.StopMoving();
        if (stopImmediately)
        {
            context.Agent.RigidBody.velocity = Vector2.zero;
            return ProcessState.Success;
        }

        return context.Agent.RigidBody.velocity.magnitude > 0.01 ? ProcessState.Running : ProcessState.Success;
    }


    protected override void OnStop() { }
}