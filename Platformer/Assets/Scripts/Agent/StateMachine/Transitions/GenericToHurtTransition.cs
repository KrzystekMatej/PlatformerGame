using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericToHurtTransition : InterruptTransition
{
    public GenericToHurtTransition() : base(StateType.Hurt) {}

    public override bool IsTriggered(AgentManager agent)
    {
        return IsInterruptAllowed(InterruptMask.Hurt, agent.StateMachine.InterruptFilter) && agent.HealthManager.IsAlive();
    }
}
