using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToIdleTransition : StateTransition
{
    public WalkToIdleTransition() : base(StateType.Idle) { }

    public override bool IsTriggered(Agent agent)
    {
        return Mathf.Abs(agent.RigidBody.velocity.x) < 0.01f;
    }
}
