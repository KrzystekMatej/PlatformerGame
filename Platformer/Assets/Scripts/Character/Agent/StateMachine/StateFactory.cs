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
        State result;
        stateTable.InnerTable.TryGetValue(stateType, out result);
        return result;
    }

    public void SetState(StateType stateType, State state)
    {
        stateTable.InnerTable[stateType] = state;
    }
}

[Serializable]
public enum StateType
{
    Idle,
    Walk,
    Fly,
    Jump,
    Fall,
    Attack,
    Climb,
    Hurt,
    Die
}
