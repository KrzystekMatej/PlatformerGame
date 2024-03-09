using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class IdleToWalkTransition : StateTransition
{
    public IdleToWalkTransition() : base(StateType.Walk) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return Mathf.Abs(agent.InputController.InputData.SteeringForce.x) > 0;
    }
}
