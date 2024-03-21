using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FiniteStateMachine : MonoBehaviour
{
    [field: SerializeField]
    public State InitialState { get; private set; }
    [field: SerializeField]
    public State CurrentState { get; private set; }

    public StateFactory Factory { get; private set; }
    public InterruptMask InterruptFilter { get; set; }
    public UnityEvent<State, State> OnTransition;


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

        StateTransition triggered = CurrentState.OrderedTransitions.FirstOrDefault(t => t.IsTriggered(agent));

        if (triggered != null)
        {
            PerformTransition(triggered, agent);
        }
        InterruptFilter = InterruptMask.None;
    }

    private void PerformTransition(StateTransition triggered, AgentManager agent)
    {
        State targetState = Factory.GetState(triggered.TargetState);

        if (targetState != null)
        {
            CurrentState.Exit();

            OnTransition?.Invoke(CurrentState, targetState);

            triggered.RunTransitionAction(agent);
            CurrentState = targetState;
            CurrentState.Enter();
        }
    }
}

