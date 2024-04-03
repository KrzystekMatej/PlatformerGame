using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericToJumpTransition : StateTransition
{
    protected GenericToJumpTransition() : base(StateType.Jump) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.InputController.InputData.Jump == InputState.Pressed && agent.GroundDetector.Detected;
    }
}
