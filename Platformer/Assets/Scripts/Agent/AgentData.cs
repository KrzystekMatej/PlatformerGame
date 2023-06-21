using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentData", menuName = "Agent/AgentData")]
public class AgentData : ScriptableObject
{
    [Header("Health data")]
    public int Health = 5;

    [Header("Run data")]
    [Space]
    public Vector2 Velocity;
    public float MaxSpeed = 6;
    public float Acceleration = 50;
    public float Deacceleration = 50;
    [Header("Jump data")]
    [Space]
    public float JumpForce = 12;
    public float JumpGravityModifier = 2;
    public float FallGravityModifier = 0.5f;
    [Header("Climb data")]
    [Space]
    public Vector2 ClimbSpeed = new Vector2(2, 5);
}