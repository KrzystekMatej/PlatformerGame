using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackToFallTransition : StateTransition
{
    protected AttackToFallTransition() : base(StateType.Fall) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return IsInterruptAllowed(InterruptMask.AnimationComplete, agent.StateMachine.InterruptFilter) && !agent.GroundDetector.Detected;
    }
}
