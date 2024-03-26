using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClimbToIdleTransition : StateTransition
{
    protected ClimbToIdleTransition() : base(StateType.Idle) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.ClimbDetector.TriggerCounter == 0 || (agent.GroundDetector.Detected && agent.InputController.InputData.SteeringForce.y == 0);
    }
}
