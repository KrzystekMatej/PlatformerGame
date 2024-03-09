using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallToIdleTransition : StateTransition
{
    public FallToIdleTransition() : base(StateType.Idle) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.GroundDetector.Detected && agent.InputController.InputData.SteeringForce.x == 0;
    }
}
