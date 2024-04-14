using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public struct SteeringOutput
{
    public ProcessState State;
    public Vector2 Force;

    public static readonly SteeringOutput Failure = new SteeringOutput(ProcessState.Failure, Vector2.zero);
    public static readonly SteeringOutput Success = new SteeringOutput(ProcessState.Success, Vector2.zero);

    private SteeringOutput(ProcessState state, Vector2 force)
    {
        State = state;
        Force = force;
    }

    public static SteeringOutput Running(Vector2 force)
    {
        return new SteeringOutput(ProcessState.Running, force);
    }
}
