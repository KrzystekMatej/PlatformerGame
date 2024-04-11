using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TransitionManager : GlobalComponent<TransitionManager>
{
    [HideInInspector]
    [SerializeReference]
    private List<StateTransition> availableTransitions = new List<StateTransition>();

    public List<StateTransition> AvailableTransitions => new List<StateTransition>(availableTransitions);

    public StateTransition GetTransitionByName(string name)
    {
        return availableTransitions.FirstOrDefault(t => t.GetType().Name == name);
    }
}