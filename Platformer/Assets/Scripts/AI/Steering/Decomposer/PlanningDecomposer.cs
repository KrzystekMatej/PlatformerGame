using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlanningDecomposer : Decomposer
{
    [SerializeField]
    private Path path;
    [SerializeField]
    private PositionQuantizer quantizer;
    
    private const int attemptCount = 8;

    private NavPath navPath;

    private void Awake()
    {
        if (quantizer.NavGraph == null) quantizer.NavGraph = FindObjectOfType<NavGraph>();
    }

    public override SteeringGoal Decompose(Agent agent, SteeringGoal goal)
    {
        if (!goal.HasPosition) return goal;
        path.Points.Clear();
        Vector2? arrivePosition = GetArrivePosition(goal.Position, agent.EnclosingCircleRadius);
        if (arrivePosition == null)
        {
            GetArrivePosition(goal.Position, agent.EnclosingCircleRadius);
            return new SteeringGoal();
        }
        goal.Position = (Vector2)arrivePosition;

        if (IsGoalVisible(agent, goal))
        {
            return goal;
        }

        NavGraphNode goalNode = QuantizeGoal(goal);
        NavGraphNode agentNode = quantizer.QuantizePosition(agent.CenterPosition, goalNode);
        navPath = CalculateNavPath(agentNode, goalNode);
        if (navPath == null) return new SteeringGoal();

        int distant = GetMostDistantReachableGoal(agent);
        int closest = GetClosestReachableNode(goal.Position, agent.EnclosingCircleRadius);

        if (distant <= closest)
        {
            path.Points.AddRange(navPath.GetExpandedPositions(agent.EnclosingCircleRadius, distant, closest + 1));
        }
        else
        {
            path.Points.AddRange(navPath.GetExpandedPositions(agent.EnclosingCircleRadius, distant, navPath.Nodes.Count));
        }


        path.Points.Add(goal.Position);

        goal.Position = path.CalculateGoalWithoutCoherence(agent);
        return goal;
    }

    private NavPath CalculateNavPath(NavGraphNode start, NavGraphNode end)
    {
        if (start == null || end == null) return null;
        if (navPath != null && navPath.GetStart() == start && navPath.GetEnd() == end) return navPath;
        return quantizer.NavGraph.FindShortestPath(start, end);
    }

    private bool IsGoalVisible(Agent agent, SteeringGoal goal)
    {
        Vector2 goalVector = goal.Position - agent.CenterPosition;
        return !Physics2D.CircleCast(agent.CenterPosition, agent.EnclosingCircleRadius, goalVector, goalVector.magnitude, quantizer.NavGraph.SolidGeometryLayerMask);
    }

    private NavGraphNode QuantizeGoal(SteeringGoal goal)
    {
        if (goal.HasOwner)
        {
            NavGraphTracker goalTracker = goal.Owner.GetComponent<NavGraphTracker>();
            if (goalTracker != null) return goalTracker.Quantizer.CoherenceNode;
        }
        quantizer.CoherenceEnabled = false;
        NavGraphNode result = quantizer.QuantizePosition(goal.Position);
        quantizer.CoherenceEnabled = true;
        return result;
    }

    private Vector2? GetArrivePosition(Vector2 goalPosition, float enclosingCircleRadius)
    {
        const float safetyMargin = 0.1f;
        float angleStep = 720;

        for (int i = 0; i < attemptCount; i++)
        {
            bool collision = Physics2D.OverlapCircle(goalPosition, enclosingCircleRadius, quantizer.NavGraph.SolidGeometryLayerMask);
            if (!collision) return goalPosition;
            for (float angle = angleStep / 2; angle <= 360; angle += angleStep)
            {
                Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.down;
                RaycastHit2D hit = Physics2D.Raycast(goalPosition, direction, enclosingCircleRadius, quantizer.NavGraph.SolidGeometryLayerMask);
                if (!hit) continue;

                Vector2 endPoint = (Vector2)MathUtility.FindLineLineIntersection(goalPosition, hit.normal, hit.point, hit.normal.Perpendicular1());
                float shiftDistance = enclosingCircleRadius - Vector2.Distance(goalPosition, endPoint) + safetyMargin;
                goalPosition += hit.normal * shiftDistance;
            } 
            angleStep /= 2;
        }

        return null;
    }

    private int GetMostDistantReachableGoal(Agent agent)
    {
        for (int i = navPath.Nodes.Count - 1; i > 0; i--)
        {
            Vector2 nodeVector = navPath.Nodes[i].GetExpandedPosition(agent.EnclosingCircleRadius) - agent.CenterPosition;
            if (!Physics2D.CircleCast(agent.CenterPosition, agent.EnclosingCircleRadius, nodeVector, nodeVector.magnitude, quantizer.NavGraph.SolidGeometryLayerMask))
            {
                return i;
            }
        }

        return 0;
    }

    private int GetClosestReachableNode(Vector2 arrivePosition, float enclosingCircleRadius)
    {
        for (int i = 0; i < navPath.Nodes.Count - 1; i++)
        {
            Vector2 nodeVector = navPath.Nodes[i].GetExpandedPosition(enclosingCircleRadius) - arrivePosition; 
            if (!Physics2D.CircleCast(arrivePosition, enclosingCircleRadius, nodeVector, nodeVector.magnitude, quantizer.NavGraph.SolidGeometryLayerMask))
            {
                return i;
            }
        }

        return navPath.Nodes.Count - 1;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        path.DrawGizmos();
    }
}
