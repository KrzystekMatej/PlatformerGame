using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public abstract class Condition : ActionNode
{
    protected abstract bool IsConditionSatisfied();

    protected override ProcessState OnUpdate()
    {
        return IsConditionSatisfied() ? ProcessState.Success : ProcessState.Failure;
    }
}
