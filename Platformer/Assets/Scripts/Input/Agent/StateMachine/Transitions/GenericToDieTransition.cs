using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericToDieTransition : StateTransition
{
    public GenericToDieTransition() : base(StateType.Die) {}

    public override bool IsTriggered(AgentManager agent)
    {
        return !agent.HealthManager.IsAlive();
    }
}
