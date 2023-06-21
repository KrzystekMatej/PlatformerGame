using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToIdleTransition : InterruptTransition
{
    public AttackToIdleTransition() : base(StateType.Idle)
    {
        interruptFilter.EnableInterrupt(InterruptType.AnimationComplete);
    }

    public override bool IsTriggered(Agent agent)
    {
        return agent.GroundDetector.CollisionDetected;
    }
}
