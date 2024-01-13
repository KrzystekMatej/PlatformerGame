using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class StopMovement : ActionNode
{
    protected override State OnUpdate()
    {
        if (context.Steering != null) context.Steering.UpdateCurrentPipeline(null);
        else context.InputController.SetMovementVector(Vector2.zero);
        return State.Success;
    }
}
