using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FiniteStateMachine : MonoBehaviour
{
    [field: SerializeField]
    public State InitialState { get; private set; }
    [field: SerializeField]
    public State CurrentState { get; private set; }

    public StateFactory Factory { get; private set; }


    private void Awake()
    {
        Factory = GetComponent<StateFactory>();
        InitialState = InitialState == null ? GetComponent<IdleState>() : InitialState;
    }

    private void Start()
    {
        Factory.InitializeStates(GetComponentInParent<AgentManager>());
        CurrentState = InitialState;
        CurrentState.Enter();
    }

    public void PerformStateUpdate(AgentManager agent)
    {
        CurrentState.HandleUpdate();

        StateTransition triggered = CurrentState.Transitions.FirstOrDefault(t => t.IsTriggered(agent));

        if (triggered != null)
        {
            PerformTransition(triggered, agent);
        }
    }

    private void PerformTransition(StateTransition triggered, AgentManager agent)
    {
        State targetState = Factory.GetState(triggered.GetTargetStateType());

        if (targetState != null)
        {
            CurrentState.Exit();
            triggered.RunTransitionAction(agent);
            CurrentState = targetState;
            CurrentState.Enter();
        }
    }

    public void PerformInterruptTransition(AgentManager agent, InterruptType interrupt)
    {
        StateTransition triggered = CurrentState.InterruptTransitions.FirstOrDefault(t => t.IsTriggered(agent) && t.IsInterruptEnabled(interrupt));

        if (triggered != null)
        {
            PerformTransition(triggered, agent);
        }
    }
}

