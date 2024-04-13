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

    private int problemSegmentIndex = -1;
    private int detectionCount;

    private NavGraphNode startNode;
    private NavGraphNode endNode;

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
        if (pointPath.Count < 2) return false;
        detector.Size = new Vector2(agent.EnclosingCircleRadius, 0);

        for (int i = 0; i < pointPath.Count - 1; i++)
        {
            Vector2? startPoint = MathUtility.GetCollisionFreePosition(pointPath[i], agent.EnclosingCircleRadius, detector.DetectLayerMask);
            Vector2? endPoint = MathUtility.GetCollisionFreePosition(pointPath[i + 1], agent.EnclosingCircleRadius, detector.DetectLayerMask);

            if (!startPoint.HasValue || !endPoint.HasValue)
            {
                problemSegmentIndex = -1;
                return true;
            }
            pointPath[i] = startPoint.Value;
            pointPath[i + 1] = endPoint.Value;

            Vector2 direction = pointPath[i + 1] - pointPath[i];
            detector.Direction = direction.normalized;
            detector.Distance = direction.magnitude;
            detectionCount = detector.Detect(pointPath[i]);

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
        if (problemSegmentIndex == -1) return new SteeringGoal();
        Collider2D obstacle = detector.Hits[0].collider;
        NavGraph navGraph = obstacle.GetComponentInChildren<NavGraph>();
        if (!navGraph) return new SteeringGoal();

        NavPath navPath = GetAvoidancePath(pointPath[problemSegmentIndex], pointPath[problemSegmentIndex + 1], agent.EnclosingCircleRadius, navGraph);
        if (navPath == null) return new SteeringGoal();

#if UNITY_EDITOR
        gizmoPath = navPath;
#endif

        goal.Position = navPath.Nodes[1].GetExpandedPosition(agent.EnclosingCircleRadius);
        return goal;
    }

    private NavPath GetAvoidancePath(Vector2 startPoint, Vector2 endPoint, float agentRadius, NavGraph navGraph)
    {
        startNode.transform.position = startPoint;
        endNode.transform.position = endPoint;

        navGraph.ConnectNode(startNode, agentRadius);
        navGraph.ConnectNode(endNode, agentRadius);

        NavPath navPath = navGraph.FindShortestPath(startNode, endNode);

        navGraph.DisconnectNode(startNode);
        navGraph.DisconnectNode(endNode);
        return navPath;
    }

#if UNITY_EDITOR
    public override void DrawGizmos()
    {
        if (!Application.isPlaying || gizmoPath == null) return;
        Gizmos.color = Color.green;
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
#endif
}
