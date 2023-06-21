using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class IdleToWalkTransition : StateTransition
{
    public IdleToWalkTransition() : base(StateType.Walk) { }

    public override bool IsTriggered(Agent agent)
    {
        return Mathf.Abs(agent.InputController.InputData.MovementVector.x) > 0;
    }
}
