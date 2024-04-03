using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public abstract class StateTransition
{
    [field: SerializeField]
    public StateType TargetState { get; protected set; }

    protected StateTransition(StateType stateType)
    {
        TargetState = stateType;
    }

    protected static bool IsInterruptAllowed(InterruptMask allowedMask, InterruptMask incomingMask)
    {
        return (allowedMask & incomingMask) > 0;
    }

    public virtual void PerformTransitionAction(AgentManager agent)
    {
        agent.Animator.PlayByType(TargetState);
    }

    public abstract bool IsTriggered(AgentManager agent);
}

