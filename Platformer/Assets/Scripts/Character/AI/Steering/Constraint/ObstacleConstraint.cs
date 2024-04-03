using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ObstacleConstraint : Constraint
{
    [SerializeField]
    private CastDetector detector;

    private int problemSegmentIndex;
    private int detectionCount;

    private NavGraphNode startNode;
    private NavGraphNode endNode;
    private List<Vector2> allowedPath = new List<Vector2>();

#if UNITY_EDITOR
    private NavPath gizmoPath;
#endif

    private void Awake()
    {
        GameObject startObject = new GameObject("startNode");
        GameObject endObject = new GameObject("endNode");
        startObject.transform.SetParent(transform);
        endObject.transform.SetParent(transform);
        startNode = startObject.AddComponent<NavGraphNode>();
        endNode = endObject.AddComponent<NavGraphNode>();
    }

    public override bool IsViolated(List<Vector2> pointPath)
    {
        if (pointPath.Count < 2 || allowedPath.SequenceEqual(pointPath)) return false;
        detector.Size = new Vector2(agent.EnclosingCircleRadius, 0);

        for (int i = 0; i < pointPath.Count - 1; i++)
        {
            Vector2 startPoint = pointPath[i];
            Vector2 endPoint = pointPath[i + 1];
            Vector2 direction = endPoint - startPoint;

            detector.Direction = direction.normalized;
            detector.Distance = direction.magnitude;
            detectionCount = detector.Detect(startPoint);

            if (detectionCount > 0)
            {
                problemSegmentIndex = i;
                return true;
            }
        }

        return false;
    }

    public override SteeringGoal Suggest(List<Vector2> pointPath, SteeringGoal goal)
    {
        Collider2D obstacle = GetClosestObstacle();
        NavGraph navGraph = obstacle.GetComponentInChildren<NavGraph>();
        float agentRadius = agent.EnclosingCircleRadius;
        int attemptCount = navGraph.RecommendedFreeCollisionAttemptCount;

        Vector2? startPoint = MathUtility.GetCollisionFreePosition(pointPath[problemSegmentIndex], agentRadius, attemptCount, navGraph.WallMask);
        Vector2? endPoint = MathUtility.GetCollisionFreePosition(pointPath[problemSegmentIndex + 1], agentRadius, attemptCount, navGraph.WallMask);

        if (startPoint == null || endPoint == null) return new SteeringGoal();

        NavPath navPath = GetAvoidancePath((Vector2)startPoint, (Vector2)endPoint, agentRadius, navGraph);

        if (navPath == null) return new SteeringGoal();

#if UNITY_EDITOR
        gizmoPath = navPath;
#endif

        goal.Position = navPath.Nodes[1].GetExpandedPosition(agent.EnclosingCircleRadius);
        allowedPath.Clear();
        allowedPath.AddRange(pointPath.Take(problemSegmentIndex + 1));
        allowedPath.Add(goal.Position);
        return goal;
    }

    private Collider2D GetClosestObstacle()
    {
        Collider2D obstacle = null;
        float shortestDistance = float.PositiveInfinity;

        for (int i = 0; i < detectionCount; i++)
        {
            if (detector.Hits[i].distance < shortestDistance)
            {
                shortestDistance = detector.Hits[i].distance;
                obstacle = detector.Hits[i].collider;
            }
        }

        return obstacle;
    }

    private NavPath GetAvoidancePath(Vector2 startPoint, Vector2 endPoint, float agentRadius, NavGraph navGraph)
    {
        startNode.transform.position = startPoint;
        endNode.transform.position = endPoint;

        startNode.ConnectToNavGraph(navGraph, agentRadius);
        endNode.ConnectToNavGraph(navGraph, agentRadius);

        NavPath navPath = navGraph.FindShortestPath(startNode, endNode);

        startNode.DisconnectFromNavGraph(navGraph);
        endNode.DisconnectFromNavGraph(navGraph);
        return navPath;
    }


    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || gizmoPath == null) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < gizmoPath.Nodes.Count - 1; i++)
        {
            Gizmos.DrawLine
            (
                gizmoPath.Nodes[i].GetExpandedPosition(agent.EnclosingCircleRadius),
                gizmoPath.Nodes[i + 1].GetExpandedPosition(agent.EnclosingCircleRadius)
            );
            Gizmos.DrawWireSphere(gizmoPath.Nodes[i].GetExpandedPosition(agent.EnclosingCircleRadius), 0.3f);
            Gizmos.DrawWireSphere(gizmoPath.Nodes[i+1].GetExpandedPosition(agent.EnclosingCircleRadius), 0.3f);
        }
    }
}
