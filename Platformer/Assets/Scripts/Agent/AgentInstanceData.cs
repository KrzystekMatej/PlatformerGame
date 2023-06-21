using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AgentInstanceData
{
    public int Health;
    public Vector2 Velocity;
    public float MaxSpeed;
    public float Acceleration;
    public float Deacceleration;
    public float JumpForce;
    public float JumpGravityModifier;
    public float FallGravityModifier;
    public Vector2 ClimbSpeed;
    public float DefaultGravityScale;
}
