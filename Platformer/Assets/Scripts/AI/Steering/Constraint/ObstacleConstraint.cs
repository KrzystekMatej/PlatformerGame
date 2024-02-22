using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ObstacleConstraint : Constraint
{
    private CastDetector detector;
    [SerializeField]
    private float margin;

    private int problemSegmentIndex;

    public override bool IsViolated(Agent agent, List<Vector2> pointPath)
    {
        if (pointPath.Count < 2) return true;
        detector.Size = new Vector2(agent.EnclosingCircleRadius, 0);

        for (int i = 0; i < pointPath.Count - 1; i++)
        {
            Vector2 startPoint = pointPath[i];
            Vector2 endPoint = pointPath[i + 1];
            Vector2 direction = endPoint - startPoint;

            detector.Direction = direction.normalized;
            detector.Distance = direction.magnitude;

            if (detector.Detect(startPoint) > 0)
            {
                problemSegmentIndex = i;
                return true;
            }
        }

        return false;
    }

    public override SteeringGoal Suggest(Agent agent, List<Vector2> pointPath, SteeringGoal goal)
    {
        RaycastHit2D closestHit = GetClosestHit();
        goal.Position = GetAvoidanceTarget(pointPath, closestHit);

        return goal;
    }

    private RaycastHit2D GetClosestHit()
    {
        RaycastHit2D closestHit = new RaycastHit2D();
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < detector.DetectionCount; i++)
        {
            if (detector.Hits[i].distance < shortestDistance)
            {
                shortestDistance = detector.Hits[i].distance;
                closestHit = detector.Hits[i];
            }
        }

        return closestHit;
    }

    private Vector2 GetAvoidanceTarget(List<Vector2> pointPath, RaycastHit2D closestHit)
    {
        Vector2 startPoint = pointPath[problemSegmentIndex];
        Vector2 endPoint = pointPath[problemSegmentIndex + 1];
        Vector2 center = closestHit.collider.bounds.center;
        float radius = MathUtility.GetEnclosingCircleRadius(closestHit.collider);

        float scalarProjection = MathUtility.GetScalarProjectionOnSegment(startPoint, endPoint, center);
        Vector2 normalPoint = startPoint + (endPoint - startPoint).normalized * scalarProjection;
        Vector2 normalDirection = (normalPoint - center).normalized;

        return center + normalDirection * radius * margin;
    }

    private void OnDrawGizmosSelected()
    {
        Agent agent = GetComponentInParent<AIInputController>().GetComponentInChildren<Agent>();
        Vector2 origin = agent.GetComponent<Collider2D>().bounds.center;
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(origin, agent.EnclosingCircleRadius);
    }
}
