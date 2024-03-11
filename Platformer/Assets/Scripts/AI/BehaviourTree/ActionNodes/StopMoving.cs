using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class StopMoving : ActionNode
{
    protected override State OnUpdate()
    {
        if (context.Steering != null) context.Steering.UpdateCurrentPipeline(null);
        context.InputController.StopMoving();
        return State.Success;
    }
}
