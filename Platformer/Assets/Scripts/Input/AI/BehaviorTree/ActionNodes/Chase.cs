using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Chase : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        Vector2 steeringForce = context.AIManager.Steering.GetContextSteeringForce();
        context.InputController.SetMovementVector(context.InputController.InputData.MovementVector + steeringForce);
        return State.Success;
    }
}
