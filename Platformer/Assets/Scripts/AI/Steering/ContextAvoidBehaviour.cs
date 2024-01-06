using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContextAvoidBehaviour : ContextSteeringBehaviour
{
    [SerializeField]
    private float obstacleMinimalDistance = 0.1f;

    private float agentColliderRadius;

    private void Start()
    {
        Vector3 colliderSize = GetComponentInParent<AIInputController>().GetComponentInChildren<Agent>().GetCenterPosition();
        if (colliderSize.x > colliderSize.y)
        {
            agentColliderRadius = colliderSize.x;
        }
        else
        {
            agentColliderRadius = colliderSize.y;
        }
    }

    public override void ModifySteeringContext(Agent agent, float[] danger, float[] interest, List<Vector2> directions)
    {
        int detectionCount = overlapDetector.Detect(agent.GetCenterPosition());
        Collider2D[] obstacles = overlapDetector.Colliders;

        for (int i = 0; i < detectionCount; i++)
        {
            UpdateDangerTowardsObstacle(agent, obstacles[i], danger, directions);
        }
    }


    private void UpdateDangerTowardsObstacle(Agent agent, Collider2D obstacle, float[] danger, List<Vector2> directions)
    {
        Vector2 directionToObstacle = obstacle.ClosestPoint(agent.GetCenterPosition()) - (Vector2)agent.GetCenterPosition();
        float distanceToObstacle = directionToObstacle.magnitude;

        float obstacleThreshold = agentColliderRadius + obstacleMinimalDistance;
        float outerRadius = overlapDetector.Size.x - obstacleThreshold;
        float distanceToObstacleFromInnerCircle = distanceToObstacle - obstacleThreshold;

        float weight = distanceToObstacle <= obstacleThreshold
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
