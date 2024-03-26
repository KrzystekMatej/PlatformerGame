using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[System.Serializable]
public class IdleToWalkTransition : StateTransition
{
    protected IdleToWalkTransition() : base(StateType.Walk) { }

    public override bool IsTriggered(AgentManager agent)
    {
        return agent.InputController.InputData.SteeringForce.x != 0;
    }
}
