using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class StopMovement : ActionNode
{
    protected override State OnUpdate()
    {
        context.InputController.SetMovementVector(Vector2.zero);
        return State.Success;
    }
}
