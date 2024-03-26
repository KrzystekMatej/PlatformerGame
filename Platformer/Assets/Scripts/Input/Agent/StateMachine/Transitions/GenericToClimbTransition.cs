using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[System.Serializable]
public class GenericToClimbTransition : StateTransition
{
    protected GenericToClimbTransition() : base(StateType.Climb) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.InputController.InputData.SteeringForce.y != 0 && agent.ClimbDetector.TriggerCounter > 0;
    }
}
