using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ContextChaseBehaviour : ContextSteeringBehaviour
{
    [SerializeField]
    private float targetReachedDistance = 0.5f;

    private Vector3? cachedTargetPosition;


    private Collider2D agentTriggerCollider;

    private void Start()
    {
        agentTriggerCollider = GetComponentInParent<Agent>().TriggerCollider;
    }

    public override void ModifySteeringContext(Agent agent, float[] danger, float[] interest, List<Vector2> directions)
    {
        int detectionCount = overlapDetector.Detect(agent.GetCenterPosition());

        int closestTargetIndex = FindClosestTarget(agent.GetCenterPosition(), detectionCount);

        if (closestTargetIndex != -1)
        {
            cachedTargetPosition = overlapDetector.Colliders[closestTargetIndex].bounds.center;
            if (ClosestTargetReached(agent, overlapDetector.Colliders[closestTargetIndex])) return;
        }
        else if (CachedTargetPositionReached(agent) || !cachedTargetPosition.HasValue)
        {
            cachedTargetPosition = null;
            return;
        }

        UpdateInterestTowardsTarget(agent, interest, directions);
    }

    private bool ClosestTargetReached(Agent agent, Collider2D targetCollider)
    {
        return Vector2.Distance(agent.GetCenterPosition(), targetCollider.bounds.center) < targetReachedDistance;
    }

    private bool CachedTargetPositionReached(Agent agent)
    {
        return cachedTargetPosition.HasValue && Vector2.Distance(agent.GetCenterPosition(), cachedTargetPosition.Value) < targetReachedDistance;
    }

    public int FindClosestTarget(Vector2 origin, int detectionCount)
    {
        float minDistance = float.MaxValue;
        int closestTargetIndex = -1;
        for (int i = 0; i < detectionCount; i++)
        {
            if (overlapDetector.Colliders[i] != null)
            {
                float distance = Vector2.Distance(overlapDetector.Colliders[i].bounds.center, origin);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTargetIndex = i;
                }

            }
        }
        return closestTargetIndex;
    }

    private void UpdateInterestTowardsTarget(Agent agent, float[] interest, List<Vector2> directions)
    {
        Vector2 directionToTarget = cachedTargetPosition.Value - agent.GetCenterPosition();

        for (int i = 0; i < interest.Length; i++)
        {
            float dot = Vector2.Dot(directionToTarget.normalized, directions[i]);

            if (dot > interest[i])
            {
                interest[i] = dot;
            }
        }
    }
}
