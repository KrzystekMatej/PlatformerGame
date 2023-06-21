using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptFilter
{
    private InterruptType allowedInterrupts = InterruptType.None;

    public void EnableInterrupt(InterruptType interrupt)
    {
        allowedInterrupts |= interrupt;
    }

    public void DisableInterrupt(InterruptType interrupt)
    {
        allowedInterrupts ^= interrupt;
    }

    public bool IsInterruptEnabled(InterruptType interrupt)
    {
        return (allowedInterrupts & interrupt) == interrupt;
    }
}

[Flags]
public enum InterruptType : short
{
    None = 0,
    AnimationAction = 1,
    AnimationComplete = 2,
    Hit = 4
};
