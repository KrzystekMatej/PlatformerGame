using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public abstract class Condition : ActionNode
{
    protected abstract bool IsConditionSatisfied();

    protected override NodeState OnUpdate()
    {
        return IsConditionSatisfied() ? NodeState.Success : NodeState.Failure;
    }
}
