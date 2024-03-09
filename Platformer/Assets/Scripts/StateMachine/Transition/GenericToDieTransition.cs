using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericToDieTransition : InterruptTransition
{
    public GenericToDieTransition() : base(StateType.Die)
    {
        interruptFilter.EnableInterrupt(InterruptType.Hit);
    }

    public override void RunTransitionAction(AgentManager agent)
    {
        agent.RigidBody.gravityScale = agent.DefaultData.GravityScale;
        agent.OnRespawnRequired.RemoveAllListeners();
    }

    public override bool IsTriggered(AgentManager agent)
    {
        return !agent.HealthManager.IsAlive();
    }
}
