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
        Vector2 steeringForce = context.AIManager.Steering.Flee((Vector3)blackboard.DataTable["OpponentPosition"]);
        if (instantaneousTurn)
        {
            context.InputController.SetMovementVector(context.InputController.InputData.MovementVector + steeringForce);
        }
        else
        {
            context.InputController.SetMovementVector(context.InputController.InputData.MovementVector + steeringForce * Time.deltaTime * turnSpeed);
        }
        return State.Success;
    }
}
