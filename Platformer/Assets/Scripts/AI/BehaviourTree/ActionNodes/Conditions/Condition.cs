using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public abstract class Condition : ActionNode
{
    protected abstract bool IsConditionSatisfied();

    protected override State OnUpdate()
    {
        return IsConditionSatisfied() ? State.Success : State.Failure;
    }
}
