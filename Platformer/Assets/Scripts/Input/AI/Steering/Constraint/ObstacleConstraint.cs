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
    [SerializeField]
    private CastDetector detector;
    [SerializeField]
    private float margin = 0.1f;

    private int problemSegmentIndex;
    private int detectionCount;

    private NavGraphNode startNode;
    private NavGraphNode endNode;

    private void Awake()
    {
        GameObject agentObject = new GameObject();
        GameObject goalObject = new GameObject();
        agentObject.transform.SetParent(transform);
        goalObject.transform.SetParent(transform);
        startNode = startNode.AddComponent<NavGraphNode>();
        endNode = startNode.AddComponent<NavGraphNode>();
    }

    public override bool IsViolated(AgentManager agent, List<Vector2> pointPath)
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
            detectionCount = detector.Detect(startPoint);

            if (detectionCount > 0)
            {
                problemSegmentIndex = i;
                return true;
            }
        }

        return false;
    }

    public override SteeringGoal Suggest(AgentManager agent, List<Vector2> pointPath, SteeringGoal goal)
    {
        RaycastHit2D closestHit = GetClosestHit();
        NavPath path = GetAvoidancePath(pointPath[problemSegmentIndex], pointPath[problemSegmentIndex + 1], agent.EnclosingCircleRadius, closestHit);

        if (path == null || path.Nodes.Count < 2) return new SteeringGoal();

        goal.Position = path.Nodes[1].GetExpandedPosition(agent.EnclosingCircleRadius);
        return goal;
    }

    private RaycastHit2D GetClosestHit()
    {
        RaycastHit2D closestHit = new RaycastHit2D();
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < detectionCount; i++)
        {
            if (detector.Hits[i].distance < shortestDistance)
            {
                shortestDistance = detector.Hits[i].distance;
                closestHit = detector.Hits[i];
            }
        }

        return closestHit;
    }

    private NavPath GetAvoidancePath(Vector2 startPoint, Vector2 endPoint, float agentRadius, RaycastHit2D obstacleHit)
    {
        NavGraph navGraph = obstacleHit.collider.GetComponentInChildren<NavGraph>();
        startNode.transform.position = startPoint;
        endNode.transform.position = endPoint;
        navGraph.AddNode(startNode);
        navGraph.AddEdges(startNode, agentRadius);
        navGraph.AddNode(endNode);
        navGraph.AddEdges(endNode, agentRadius);
        NavPath navPath = navGraph.FindShortestPath(startNode, endNode);

        navGraph.RemoveNode(startNode);
        navGraph.RemoveNode(endNode);
        startNode.Neighbors.Clear();
        endNode.Neighbors.Clear();
        return navPath;
    }

    private void OnDrawGizmosSelected()
    {
        AgentManager agent = GetComponentInParent<AIInputController>().GetComponentInChildren<AgentManager>();
        
    }
}
