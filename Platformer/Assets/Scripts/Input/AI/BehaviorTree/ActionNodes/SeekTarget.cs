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

    public override void Initialize()
    {
        Vector3 target = (Vector3)blackboard.DataTable["CurrentTarget"];
        context.InputController.SetMovementVector(context.AIManager.Steering.Seek(target));
    }

    protected override State OnUpdate()
    {
        Vector3 target = (Vector3)blackboard.DataTable["CurrentTarget"];
        Vector2 steeringForce = context.AIManager.Steering.Seek(target);
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
