using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContextAvoidBehaviour : ContextSteeringBehaviour
{
    [SerializeField]
    private float obstacleMinimalDistance = 0.1f;

    private Collider2D agentTriggerCollider;
    private float agentColliderRadius;


    private void Awake()
    {
        
        areaDetector = transform.parent.GetComponentInChildren<ObstacleDetector>();
    }

    private void Start()
    {
        agentTriggerCollider = GetComponentInParent<Agent>().TriggerCollider;
        Vector3 colliderSize = agentTriggerCollider.bounds.extents;
        if (colliderSize.x > colliderSize.y)
        {
            agentColliderRadius = colliderSize.x;
        }
        else
        {
            agentColliderRadius = colliderSize.y;
        }
    }

    public override void ModifySteeringContext(float[] danger, float[] interest, List<Vector2> directions)
    {
        Collider2D[] obstacles = areaDetector.GetColliders();

        for (int i = 0; i < areaDetector.ColliderCount; i++)
        {
            UpdateDangerTowardsObstacle(obstacles[i], danger, directions);
        }
    }


    private void UpdateDangerTowardsObstacle(Collider2D obstacle, float[] danger, List<Vector2> directions)
    {
        Vector2 directionToObstacle = obstacle.ClosestPoint(transform.position) - (Vector2)transform.position;
        float distanceToObstacle = directionToObstacle.magnitude;

        float obstacleThreshold = agentColliderRadius + obstacleMinimalDistance;
        float outerRadius = areaDetector.DetectionRadius - obstacleThreshold;
        float distanceToObstacleFromInnerCircle = distanceToObstacle - obstacleThreshold;

        float weight = distanceToObstacle <= agentColliderRadius + obstacleMinimalDistance
            ? 1
            : (outerRadius - distanceToObstacleFromInnerCircle) / outerRadius;

        Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

        for (int i = 0; i < directions.Count; i++)
        {
            float dot = Vector2.Dot(directionToObstacleNormalized, directions[i]);
            float result = dot * weight;
            if (result > danger[i])
            {
                danger[i] = result;
            }
        }
    }
}
