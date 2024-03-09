using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallToWalkTransition : StateTransition
{
    public FallToWalkTransition() : base(StateType.Walk) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.GroundDetector.Detected && agent.InputController.InputData.SteeringForce.x != 0;
    }
}
