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
        state = NodeState.Running;
    }

    private void AttackFinished(State previous, State next)
    {
        if (previous is AttackState) state = NodeState.Success;
    }

    protected override NodeState OnUpdate()
    {
        return state;
    }

    protected override void OnStop() { }
}
