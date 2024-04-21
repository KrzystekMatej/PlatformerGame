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
    private LayerMask obstacleMask;

    private int problemSegmentIndex = -1;
    private RaycastHit2D hit;

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

        for (int i = 0; i < pointPath.Count - 1; i++)
        {
            Vector2? nonBlockStart = MathUtility.UnblockPosition(pointPath[i], agent.EnclosingCircleRadius, obstacleMask);
            Vector2? nonBlockEnd = MathUtility.UnblockPosition(pointPath[i + 1], agent.EnclosingCircleRadius, obstacleMask);

            if (!nonBlockStart.HasValue || !nonBlockEnd.HasValue)
            {
                problemSegmentIndex = -1;
                return true;
            }

            pointPath[i] = nonBlockStart.Value;
            pointPath[i + 1] = nonBlockEnd.Value;

            Vector2 segmentVector = pointPath[i + 1] - pointPath[i];
            hit = Physics2D.CircleCast(pointPath[i], agent.EnclosingCircleRadius, segmentVector, segmentVector.magnitude, obstacleMask);

            if (hit)
            {
                problemSegmentIndex = i;
                return true;
            }
        }

        return false;
    }

    public override bool Suggest(List<Vector2> pointPath, SteeringGoal goal)
    {
        if (problemSegmentIndex == -1) return false;

        NavGraph navGraph = hit.collider.GetComponentInChildren<NavGraph>();
        if (!navGraph) return false;

        NavPath navPath = GetAvoidancePath(pointPath[problemSegmentIndex], pointPath[problemSegmentIndex + 1], agent.EnclosingCircleRadius, navGraph);
        if (navPath == null) return false;

#if UNITY_EDITOR
        gizmoPath = navPath;
#endif

        goal.Position = navPath.Nodes[1].GetExpandedPosition(agent.EnclosingCircleRadius);
        return true;
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
