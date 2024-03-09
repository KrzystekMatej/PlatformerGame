using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToIdleTransition : InterruptTransition
{
    public AttackToIdleTransition() : base(StateType.Idle)
    {
        interruptFilter.EnableInterrupt(InterruptType.AnimationComplete);
    }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.GroundDetector.Detected;
    }
}
