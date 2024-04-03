using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClimbToJumpTransition : StateTransition
{
    protected ClimbToJumpTransition() : base(StateType.Jump) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.InputController.InputData.Jump == InputState.Pressed;
    }
}