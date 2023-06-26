using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Attack : ActionNode
{
    protected override State OnUpdate()
    {
        context.InputController.Attack();
        return State.Success;
    }
}
