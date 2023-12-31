using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateTransition
{
    public StateType StateType { get; }

    public StateTransition(StateType stateType)
    {
        StateType = stateType;
    }

    public StateType GetTargetStateType()
    {
        return StateType;
    }

    public virtual void RunTransitionAction(Agent agent) {}

    public abstract bool IsTriggered(Agent agent);
}

