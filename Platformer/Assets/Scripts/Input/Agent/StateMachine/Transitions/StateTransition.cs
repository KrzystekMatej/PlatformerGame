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

    public StateTransition(StateType stateType)
    {
        TargetState = stateType;
    }

    public virtual void RunTransitionAction(AgentManager agent)
    {
        agent.Animator.PlayByType(TargetState);
    }

    public abstract bool IsTriggered(AgentManager agent);
}

