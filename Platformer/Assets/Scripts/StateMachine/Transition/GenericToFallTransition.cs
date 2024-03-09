using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericToFallTransition : StateTransition
{
    public GenericToFallTransition() : base(StateType.Fall) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return !agent.GroundDetector.Detected;
    }
}