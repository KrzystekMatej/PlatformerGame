using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericToFallTransition : StateTransition
{
    protected GenericToFallTransition() : base(StateType.Fall) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return !agent.GroundDetector.Detected;
    }
}