using System;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class PursueTargeter : SeekTargeter
{
    [field: SerializeField]
    public float MaxPrediction { get; private set; }

    public override ProcessState Target(SteeringGoal goal)
    {

        float speed = Agent.RigidBody.velocity.magnitude;
        float distance = (GoalPosition - Agent.PhysicsCenter).magnitude;
        float prediction = speed <= distance / MaxPrediction ? MaxPrediction : distance / speed;

        Vector2 targetVelocity = Vector2.zero;
        if (GoalOwner)
        {
            Rigidbody2D rigidBody = GoalOwner.GetComponent<Rigidbody2D>();
            if (rigidBody) targetVelocity = rigidBody.velocity;
        }

        Vector2 futureOffset = targetVelocity * prediction;

        RaycastHit2D hit = Physics2D.Raycast(GoalPosition, targetVelocity, futureOffset.magnitude, Agent.PhysicsMask);
        if (hit)
        {
            const float safetyMargin = 0.001f;
            GoalPosition = hit.point + hit.normal * (Agent.PhysicsRadius + safetyMargin);
        }
        else GoalPosition += futureOffset;

        return base.Target(goal);
    }
}