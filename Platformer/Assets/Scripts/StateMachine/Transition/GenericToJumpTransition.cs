using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericToJumpTransition : StateTransition
{
    public GenericToJumpTransition() : base(StateType.Jump) { }

    public override bool IsTriggered(Agent agent)
    {
        return agent.InputController.InputData.Jump == InputState.Pressed && agent.GroundDetector.CollisionDetected;
    }
}
