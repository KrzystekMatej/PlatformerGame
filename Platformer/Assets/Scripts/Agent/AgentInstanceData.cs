using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AgentInstanceData
{
    public int Health;
    public Vector2 Acceleration;
    public float MaxSpeed;
    public float MaxForce;
    public float JumpForce;
    public float JumpGravityModifier;
    public float FallGravityModifier;
    public Vector2 ClimbSpeed;
}
