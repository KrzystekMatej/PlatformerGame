using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SeekTarget : ActionNode
{
    [SerializeField]
    private float turnSpeed = 1f;
    [SerializeField]
    private bool instantaneousTurn;

    protected override State OnUpdate()
    {
        Vector3 target = (Vector3)blackboard.DataTable["CurrentTarget"];
        Vector2 desiredVelocity = context.Steering.Seek(target);
        if (instantaneousTurn) 
        {
            context.InputController.SetMovementVector(desiredVelocity);
        }
        else
        {
            Vector2 steeringForce = desiredVelocity - context.Agent.RigidBody.velocity;
            context.InputController.SetMovementVector((context.Agent.RigidBody.velocity + steeringForce * Time.deltaTime * turnSpeed).normalized);
        }
        return State.Success;
    }
}
