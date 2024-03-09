using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToFallTransition : InterruptTransition
{
    public AttackToFallTransition() : base(StateType.Fall)
    {
        interruptFilter.EnableInterrupt(InterruptType.AnimationComplete);
    }

    public override bool IsTriggered(AgentManager agent)
    {
        return !agent.GroundDetector.Detected;
    }
}
