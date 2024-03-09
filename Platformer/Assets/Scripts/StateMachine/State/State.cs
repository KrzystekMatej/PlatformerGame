using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class State : MonoBehaviour
{
    public StateTransition[] Transitions { get; private set; }
    public InterruptTransition[] InterruptTransitions { get; private set; }

    protected AgentManager agent;
    public UnityEvent OnEnter, OnExit;

    public void Initialize(AgentManager agent)
    {
        this.agent = agent;
        SetTransitions(GetTransitions());
    }

    private void SetTransitions(StateTransition[] allTransitions)
    {
        Transitions = allTransitions.Where(t => !(t is InterruptTransition)).ToArray();
        InterruptTransitions = allTransitions.OfType<InterruptTransition>().ToArray();
    }

    protected abstract StateTransition[] GetTransitions();

    public void Enter()
    {
        OnEnter?.Invoke();
        HandleEnter();
    }

    protected virtual void HandleEnter() { }

    public virtual void HandleUpdate() { }
    
    public void Exit()
    {
        OnExit?.Invoke();
        HandleExit();
    }

    protected virtual void HandleExit() { }
}
