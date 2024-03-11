using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum InterruptMask : short
{
    None = 0,
    AnimationAction = 1,
    AnimationComplete = 2,
    Hurt = 4
};
