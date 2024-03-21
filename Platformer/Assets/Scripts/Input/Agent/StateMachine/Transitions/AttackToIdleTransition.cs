using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackToIdleTransition : InterruptTransition
{
    public AttackToIdleTransition() : base(StateType.Idle) {}

    public override bool IsTriggered(AgentManager agent)
    {
        return IsInterruptAllowed(InterruptMask.AnimationComplete, agent.StateMachine.InterruptFilter) && agent.GroundDetector.Detected;
    }
}
