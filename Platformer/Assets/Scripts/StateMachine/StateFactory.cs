using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateFactory : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<StateType, State> stateTable = new SerializableDictionary<StateType, State>();

    public State GetState(StateType stateType)
    {
        if (stateTable.ContainsKey(stateType))
        {
            return stateTable[stateType];
        }
        return null;
    }

    public void SetState(StateType stateType, State state)
    {
        stateTable[stateType] = state;
    }



    public void InitializeStates(Agent agent)
    {
        foreach (State state in GetComponents<State>())
        {
            state.Initialize(agent);
        }
    }
}

public enum StateType
{
    Idle,
    Walk,
    Fly,
    Jump,
    Fall,
    Attack,
    Climb,
    Crouch,
    Hurt,
    Die
}
