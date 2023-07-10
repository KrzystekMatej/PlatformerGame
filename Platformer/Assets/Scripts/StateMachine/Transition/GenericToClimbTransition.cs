using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GenericToClimbTransition : StateTransition
{
    public GenericToClimbTransition() : base(StateType.Climb) { }

    public override bool IsTriggered(Agent agent)
    {
        return Mathf.Abs(agent.InputController.InputData.MovementVector.y) > 0 && agent.ClimbDetector.TriggerCounter > 0;
    }
}
