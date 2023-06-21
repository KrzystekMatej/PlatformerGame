using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterruptTransition : StateTransition
{
    protected InterruptFilter interruptFilter = new InterruptFilter();

    public InterruptTransition(StateType stateType) : base(stateType) { }

    public bool IsInterruptEnabled(InterruptType interrupt)
    {
        return interruptFilter.IsInterruptEnabled(interrupt);
    }
}
