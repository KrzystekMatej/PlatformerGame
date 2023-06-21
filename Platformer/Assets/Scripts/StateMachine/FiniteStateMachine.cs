using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField]
    private State initialState;
    [SerializeField]
    private State currentState;

    public StateFactory Factory { get; private set; }


    private void Awake()
    {
        Factory = GetComponent<StateFactory>();
        initialState = initialState == null ? GetComponent<IdleState>() : initialState;
    }

    private void Start()
    {
        Factory.InitializeStates(GetComponentInParent<Agent>());
        currentState = initialState;
        currentState.Enter();
    }

    public void UpdateState(Agent agent)
    {
        StateTransition triggered = currentState.Transitions.FirstOrDefault(t => t.IsTriggered(agent));

        if (triggered != null)
        {
            PerformTransition(triggered, agent);
        }

        currentState.HandleUpdate();
    }

    private void PerformTransition(StateTransition triggered, Agent agent)
    {
        State targetState = Factory.GetState(triggered.GetTargetStateType());

        if (targetState != null)
        {
            currentState.Exit();
            triggered.RunTransitionAction(agent);
            currentState = targetState;
            currentState.Enter();
        }
    }

    public void PerformInterruptTransition(Agent agent, InterruptType interrupt)
    {
        StateTransition triggered = currentState.InterruptTransitions.FirstOrDefault(t => t.IsTriggered(agent) && t.IsInterruptEnabled(interrupt));

        if (triggered != null)
        {
            PerformTransition(triggered, agent);
        }
    }
}

