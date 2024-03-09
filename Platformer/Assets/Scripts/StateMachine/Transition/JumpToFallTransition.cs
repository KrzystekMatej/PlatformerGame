using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToFallTransition : StateTransition
{
    public JumpToFallTransition() : base(StateType.Fall) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.RigidBody.velocity.y <= 0;
    }
}

