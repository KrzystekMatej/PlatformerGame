using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class WalkForward : ActionNode
{
    public override void Initialize()
    {
        float currentHorizontalInput = context.Agent.InputController.InputData.MovementVector.x;
        float currentAgentOrientation = context.Agent.OrientationController.CurrentOrientation;
        blackboard.DataTable["HorizontalDirection"] = currentHorizontalInput != 0 ? currentHorizontalInput : currentAgentOrientation;
    }


    protected override State OnUpdate()
    {
        context.InputController.SetMovementVector(new Vector2((float)blackboard.DataTable["HorizontalDirection"], 0));
        return State.Success;
    }
}
