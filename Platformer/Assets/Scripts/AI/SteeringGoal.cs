using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringGoal
{
    private bool hasPosition;
    private bool hasVelocity;

    private Vector2 position;
    private Vector2 velocity;

    public void UpdateChannels(SteeringGoal other)
    {
        if (hasPosition)
        {
            position = other.position;
            hasPosition = true;
        }
        if (hasVelocity)
        {
            velocity = other.velocity;
            hasVelocity = true;
        }
    }
}
