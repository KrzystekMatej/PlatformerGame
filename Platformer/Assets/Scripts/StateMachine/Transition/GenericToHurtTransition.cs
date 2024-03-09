using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericToHurtTransition : InterruptTransition
{
    public GenericToHurtTransition() : base(StateType.Hurt)
    {
        interruptFilter.EnableInterrupt(InterruptType.Hit);
    }

    public override void RunTransitionAction(AgentManager agent)
    {
        if (agent.Invulnerability != null)
        {
            agent.StartCoroutine(agent.Invulnerability.Run(agent.StateMachine.Factory));
        }
    }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.HealthManager.IsAlive() && (agent.Invulnerability == null || (agent.Invulnerability != null && !agent.Invulnerability.IsActive));
    }
}
