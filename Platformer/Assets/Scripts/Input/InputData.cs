using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputData
{
    public InputState Jump;
    public InputState Attack;
    public InputState WeaponSwap;
    public InputState Crouch;
    public Vector2 MovementVector;
}

public enum InputState
{
    Inactive,
    Pressed,
    Held,
    Released
}
