using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtToIdleTransition : InterruptTransition
{
    public HurtToIdleTransition() : base(StateType.Idle)
    {
        interruptFilter.EnableInterrupt(InterruptType.AnimationComplete);
    }

    public override bool IsTriggered(AgentManager agent)
    {
        return true;
    }
}
