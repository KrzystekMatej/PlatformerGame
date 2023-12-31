using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallToIdleTransition : StateTransition
{
    public FallToIdleTransition() : base(StateType.Idle) { }

    public override bool IsTriggered(Agent agent)
    {
        return agent.GroundDetector.CollisionDetected && agent.InputController.InputData.MovementVector.x == 0;
    }
}
