using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbToJumpTransition : StateTransition
{
    public ClimbToJumpTransition() : base(StateType.Jump) { }

    public override bool IsTriggered(Agent agent)
    {
        return agent.InputController.InputData.Jump == InputState.Pressed;
    }
}