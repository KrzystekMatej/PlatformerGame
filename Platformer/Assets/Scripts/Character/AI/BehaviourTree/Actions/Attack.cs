using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class Attack : ActionNode
{
    private bool transitionSatisfied = false;

    protected override void OnStart()
    {
        context.InputController.Attack();
        context.Agent.StateMachine.OnTransition.AddListener(AttackFinished);
        state = ProcessState.Running;
    }

    private void AttackFinished(State previous, State next)
    {
        if (transitionSatisfied) state = ProcessState.Success;
        else if (next is AttackState) transitionSatisfied = true;
        else state = ProcessState.Failure;
    }

    protected override ProcessState OnUpdate()
    {
        return state;
    }

    protected override void OnStop()
    {
        context.Agent.StateMachine.OnTransition.RemoveListener(AttackFinished);
        transitionSatisfied = false;
    }
}
