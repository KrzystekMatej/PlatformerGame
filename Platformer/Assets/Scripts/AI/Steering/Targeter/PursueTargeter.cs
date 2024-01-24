using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargeter : SeekTargeter
{
    [field: SerializeField]
    public float MaxPrediction { get; private set; }
    public Vector2 TargetVelocity { get; set; }

    public override SteeringGoal GetGoal(Agent agent)
    {
        float distance = (TargetPosition - agent.CenterPosition).magnitude;
        float speed = agent.RigidBody.velocity.magnitude;
        float prediction = speed <= distance / MaxPrediction ? MaxPrediction : distance / speed;

        Vector2 futurePosition = agent.CenterPosition + TargetVelocity * prediction;

        TargetPosition = futurePosition;
        TargetVelocity = Vector2.zero;

        return base.GetGoal(agent);
    }
}