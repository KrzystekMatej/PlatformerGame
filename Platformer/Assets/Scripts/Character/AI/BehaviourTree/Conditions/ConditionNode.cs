using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public abstract class ConditionNode : ActionNode
{
    protected sealed override void OnStart() { }

    protected abstract bool IsConditionSatisfied();

    protected sealed override void OnStop() { }

    protected override ProcessState OnUpdate()
    {
        return IsConditionSatisfied() ? ProcessState.Success : ProcessState.Failure;
    }
}
