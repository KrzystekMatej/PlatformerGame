using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Flee : ActionNode
{
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private bool instantaneousTurn;

    protected override State OnUpdate()
    {
        Vector2 desiredVelocity = context.AIManager.Steering.Flee((Vector3)blackboard.DataTable["OpponentPosition"]);
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
