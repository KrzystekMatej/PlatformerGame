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


    private void Awake()
    {

        areaDetector = transform.parent.GetComponentInChildren<TargetDetector>();
    }

    private void Start()
    {
        agentTriggerCollider = GetComponentInParent<Agent>().TriggerCollider;
    }

    public override void ModifySteeringContext(float[] danger, float[] interest, List<Vector2> directions)
    {
        Collider2D[] targets = areaDetector.GetColliders();

        int closestTargetIndex = FindClosestTarget(targets);

        if (closestTargetIndex != -1)
        {
            cachedTargetPosition = targets[closestTargetIndex].bounds.center;
            if (ClosestTargetReached(targets[closestTargetIndex])) return;
        }
        else if (CachedTargetPositionReached() || !cachedTargetPosition.HasValue)
        {
            cachedTargetPosition = null;
            return;
        }

        UpdateInterestTowardsTarget(interest, directions);
    }

    private bool ClosestTargetReached(Collider2D targetCollider)
    {
        Debug.Log(Vector2.Distance(transform.position, targetCollider.bounds.center));
        return Vector2.Distance(transform.position, targetCollider.bounds.center) < targetReachedDistance;
    }

    private bool CachedTargetPositionReached()
    {
        if (cachedTargetPosition != null)
        {
            Debug.Log(Vector2.Distance(transform.position, cachedTargetPosition.Value));
        }
        return cachedTargetPosition.HasValue && Vector2.Distance(transform.position, cachedTargetPosition.Value) < targetReachedDistance;
    }

    public int FindClosestTarget(Collider2D[] targets)
    {
        float minDistance = float.MaxValue;
        int closestTargetIndex = -1;
        for (int i = 0; i < areaDetector.ColliderCount; i++)
        {
            if (targets[i] != null)
            {
                float distance = Vector2.Distance(targets[i].bounds.center, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTargetIndex = i;
                }

            }
        }
        return closestTargetIndex;
    }

    private void UpdateInterestTowardsTarget(float[] interest, List<Vector2> directions)
    {
        Vector2 directionToTarget = cachedTargetPosition.Value - transform.position;

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
