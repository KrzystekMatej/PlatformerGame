using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringGoal
{
    public bool HasPosition { get; private set; }
    public bool HasVelocity { get; private set; }
    public bool HasOwner { get; private set; }

    private Vector2 position;
    private Vector2 velocity;
    private GameObject owner;

    public Vector2 Position
    {
        get { return position; }
        set
        {
            position = value;
            HasPosition = true;
        }
    }

    public Vector2 Velocity
    {
        get { return velocity; }
        set
        {
            velocity = value;
            HasPosition = true;
        }
    }

    public GameObject Owner
    {
        get { return owner; }
        set
        {
            owner = value;
            HasOwner = owner != null;
        }
    }

    public bool HasNothing()
    {
        return !HasPosition && !HasVelocity && !HasOwner;
    }

    public void UpdateChannels(SteeringGoal other)
    {
        if (other.HasPosition)
        {
            Position = other.Position;
        }
         
        if (other.HasVelocity)
        {
            Velocity = other.Velocity;
        }

        if (other.HasOwner)
        {
            Owner = other.Owner;
        }
    }
}
