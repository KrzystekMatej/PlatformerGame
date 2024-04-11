using System;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class PursueTargeter : SeekTargeter
{
    [field: SerializeField]
    public float MaxPrediction { get; private set; }
    [SerializeField]
    LayerMask physicsMask;

    public override void Enable()
    {
        physicsMask = Utility.GetCollisionLayerMask(agent.PhysicsCollider.gameObject.layer);
    }

    public override ProcessState TryUpdateGoal(SteeringGoal goal)
    {
        Vector2? collisionFreeGoal = MathUtility.GetCollisionFreePosition
        (
            GoalPosition,
            agent.EnclosingCircleRadius,
            physicsMask
        );
        if (!collisionFreeGoal.HasValue) return ProcessState.Failure;
        GoalPosition = collisionFreeGoal.Value;

        float speed = agent.RigidBody.velocity.magnitude;
        float distance = (GoalPosition - agent.CenterPosition).magnitude;
        float prediction = speed <= distance / MaxPrediction ? MaxPrediction : distance / speed;

        Vector2 targetVelocity = Vector2.zero;
        if (GoalOwner)
        {
            Rigidbody2D rigidBody = GoalOwner.GetComponent<Rigidbody2D>();
            if (rigidBody) targetVelocity = rigidBody.velocity;
        }

        Vector2 futureOffset = targetVelocity * prediction;

        RaycastHit2D hit = Physics2D.CircleCast(GoalPosition, agent.EnclosingCircleRadius, targetVelocity, futureOffset.magnitude, physicsMask);
        if (hit)
        {
            const float safetyMargin = 0.001f;
            GoalPosition = hit.point + hit.normal * (agent.EnclosingCircleRadius + safetyMargin);
        }
        else GoalPosition += futureOffset;

        return base.TryUpdateGoal(goal);
    }
}