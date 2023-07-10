using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbToIdleTransition : StateTransition
{

    public ClimbToIdleTransition() : base(StateType.Idle) { }

    public override bool IsTriggered(Agent agent)
    {
        return agent.ClimbDetector.TriggerCounter == 0;
    }
}
