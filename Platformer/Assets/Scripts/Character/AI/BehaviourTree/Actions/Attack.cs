using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class Attack : ActionNode
{
    protected override void OnStart()
    {
        context.InputController.Attack();
        context.Agent.StateMachine.OnTransition.AddListener(AttackFinished);
        state = ProcessState.Running;
    }

    private void AttackFinished(State previous, State next)
    {
        if (next is not AttackState) state = ProcessState.Failure;
        if (previous is AttackState) state = ProcessState.Success;
    }

    protected override ProcessState OnUpdate()
    {
        return state;
    }

    protected override void OnStop()
    {
        context.Agent.StateMachine.OnTransition.RemoveListener(AttackFinished);
    }
}
