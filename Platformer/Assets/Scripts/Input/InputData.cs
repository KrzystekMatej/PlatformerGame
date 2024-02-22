using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputData
{
    public Vector2 SteeringForce;
    public InputState Jump;
    public InputState Attack;
    public InputState WeaponSwap;
    public InputState Crouch;
}

public enum InputState
{
    Inactive,
    Pressed,
    Held,
    Released
}
