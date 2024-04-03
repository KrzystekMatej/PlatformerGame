using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClimbToFallTransition : StateTransition
{
    protected ClimbToFallTransition() : base(StateType.Fall) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.ClimbDetector.TriggerCounter == 0 && !agent.GroundDetector.Detected;
    }
}
