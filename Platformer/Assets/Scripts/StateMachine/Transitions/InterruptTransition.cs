using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class InterruptTransition : StateTransition
{
    public InterruptTransition(StateType targetState) : base(targetState) { }

    public static bool IsInterruptAllowed(InterruptMask allowedMask, InterruptMask incomingMask)
    {
        return (allowedMask & incomingMask) > 0;
    }
}
