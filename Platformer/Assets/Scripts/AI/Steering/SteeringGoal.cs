using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringGoal
{
    public bool HasPosition { get; private set; }
    public bool HasVelocity { get; private set; }

    private Vector2 position;
    private Vector2 velocity;

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
    }
}
