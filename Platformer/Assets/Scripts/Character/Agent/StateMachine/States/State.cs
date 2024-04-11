using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class State : MonoBehaviour
{
    [SerializeField]
    protected AgentManager agent;
    [HideInInspector]
    [SerializeReference]
    private List<StateTransition> orderedTransitions = new List<StateTransition>();

    public List<StateTransition> OrderedTransitions => orderedTransitions;
    public UnityEvent OnEnter, OnUpdate, OnExit;

    public void Awake()
    {
        agent = agent ? agent : GetComponentInParent<AgentManager>();
    }

    public void Start()
    {
        InitializeTransitions();
    }

    private void InitializeTransitions()
    {
        List<StateTransition> globalTransitions = new List<StateTransition>();
        for (int i = 0; i < orderedTransitions.Count; i++)
        {
            StateTransition transition = TransitionManager.Instance.GetTransitionByName(orderedTransitions[i].GetType().Name);
            if (transition != null) globalTransitions.Add(transition);
        }
        orderedTransitions.Clear();
        orderedTransitions.AddRange(globalTransitions);
    }

    public void PerformEnterActions()
    {
        HandleEnter();
        OnEnter?.Invoke();
    }

    protected abstract void HandleEnter();


    public void PerformUpdateActions()
    {
        HandleUpdate();
        OnUpdate?.Invoke();
    }

    protected abstract void HandleUpdate();
    
    public void PerformExitActions()
    {
        HandleExit();
        OnExit?.Invoke();
    }

    protected abstract void HandleExit();
}

