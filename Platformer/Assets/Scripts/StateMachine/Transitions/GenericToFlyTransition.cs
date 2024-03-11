using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericToFlyTransition : InterruptTransition
{
    public GenericToFlyTransition() : base(StateType.Fly) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return IsInterruptAllowed(InterruptMask.AnimationComplete, agent.StateMachine.InterruptFilter) && (agent.GroundDetector == null || !agent.GroundDetector.Detected);
    }
}