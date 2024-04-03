using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HurtToIdleTransition : StateTransition
{
    protected HurtToIdleTransition() : base(StateType.Idle) {}

    public override bool IsTriggered(AgentManager agent)
    {
        return IsInterruptAllowed(InterruptMask.AnimationComplete, agent.StateMachine.InterruptFilter);
    }
}
