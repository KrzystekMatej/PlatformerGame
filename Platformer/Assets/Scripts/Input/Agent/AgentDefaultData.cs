using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentData", menuName = "Agent/AgentData")]
public class AgentDefaultData : ScriptableObject
{
    [field: Header("Health data")]
    [field: SerializeField]
    public int Health { get; private set; } = 5;

    [field: Header("Run data")]
    [field: Space]
    [field: SerializeField]
    public float MaxSpeed { get; private set; } = 6;
    [field: SerializeField]
    public float MaxForce { get; private set; } = 50;
    [field: Header("Jump data")]
    [field: Space]
    [field: SerializeField]
    public float JumpForce { get; private set; } = 12;
    [field: SerializeField]
    public float JumpGravityModifier { get; private set; } = 2;
    [field: SerializeField]
    public float FallGravityModifier { get; private set; } = 0.5f;
    [field: Header("Climb data")]
    [field: Space]
    [field: SerializeField]
    public Vector2 ClimbSpeed { get; private set; } = new Vector2(2, 5);
    [field: Header("General data")]
    [field: Space]
    [field: SerializeField]
    public float GravityScale { get; private set; } = 2;
}