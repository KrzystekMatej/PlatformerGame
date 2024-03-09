using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToIdleTransition : StateTransition
{
    public WalkToIdleTransition() : base(StateType.Idle) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return Mathf.Abs(agent.RigidBody.velocity.x) < Mathf.Epsilon && agent.InputController.InputData.SteeringForce.x == 0;
    }
}
