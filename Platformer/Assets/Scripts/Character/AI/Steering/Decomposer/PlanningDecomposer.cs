using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlanningDecomposer : Decomposer
{
    private NavPath navPath;

    private NavGraphTracker agentTracker;
#if UNITY_EDITOR
    private Vector2 gizmoAgentPosition;
    private Vector2 gizmoGoalPosition;
#endif

    protected override void Start()
    {
        base.Start();
        agentTracker = agent.GetComponent<NavGraphTracker>();
    }

    public override SteeringGoal Decompose(SteeringGoal goal)
    {
        if (!goal.HasPosition) return goal;
        float agentRadius = agent.EnclosingCircleRadius;
        int attemptCount = agentTracker.NavGraph.RecommendedFreeCollisionAttemptCount;

        Vector2? agentPosition = MathUtility.GetCollisionFreePosition(agent.CenterPosition, agentRadius, attemptCount, agentTracker.NavGraph.WallMask);
        Vector2? goalPosition = MathUtility.GetCollisionFreePosition(goal.Position, agentRadius, attemptCount, agentTracker.NavGraph.WallMask);

        if (agentPosition == null || goalPosition == null) return new SteeringGoal();
        goal.Position = (Vector2)goalPosition;

#if UNITY_EDITOR
        gizmoAgentPosition = (Vector2)agentPosition;
        gizmoGoalPosition = (Vector2)goalPosition;
#endif

        if (IsGoalVisible((Vector2)agentPosition, goal.Position, agentRadius))
        {
            return goal;
        }

        navPath = GetNavPath(goal);
        if (navPath == null) return new SteeringGoal();

        goal.Position = navPath.Nodes[GetMostDistantReachableGoal((Vector2)agentPosition, agentRadius)].GetExpandedPosition(agentRadius);
        return goal;
    }

    private bool IsGoalVisible(Vector2 agentPosition, Vector2 goalPosition, float agentRadius)
    {
        Vector2 goalVector = goalPosition - agentPosition;
        return !Physics2D.CircleCast(agentPosition, agentRadius, goalVector, goalVector.magnitude, agentTracker.NavGraph.WallMask);
    }

    private NavPath GetNavPath(SteeringGoal goal)
    {
        NavGraphNode startNode = agentTracker.Current;
        NavGraphNode endNode = QuantizeGoal(goal);


        
        if (startNode && endNode)
        {
            NavPath navPath = agentTracker.NavGraph.FindShortestPath(startNode, endNode);
            return navPath;
        }
        else return null;
    }

    private NavGraphNode QuantizeGoal(SteeringGoal goal)
    {
        if (goal.HasOwner)
        {
            NavGraphTracker tracker = goal.Owner.GetComponents<NavGraphTracker>().FirstOrDefault(t => t.NavGraph == agentTracker.NavGraph);
            if (tracker && tracker.NavGraph == agentTracker.NavGraph) return tracker.Current;
        }

        return agentTracker.NavGraph.QuantizePosition(goal.Position);
    }

    private int GetMostDistantReachableGoal(Vector2 agentPosition, float enclosingCircleRadius)
    {
        for (int i = navPath.Nodes.Count - 1; i > 0; i--)
        {
            Vector2 nodeVector = navPath.Nodes[i].GetExpandedPosition(enclosingCircleRadius) - agentPosition;
            if (!Physics2D.CircleCast(agentPosition, enclosingCircleRadius, nodeVector, nodeVector.magnitude, agentTracker.NavGraph.WallMask))
            {
                return i;
            }
        }

        return 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        if (navPath == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoAgentPosition, 0.3f);
        Gizmos.DrawWireSphere(gizmoGoalPosition, 0.3f);
        Gizmos.color = Color.yellow;
        for (int i = 0; i < navPath.Nodes.Count - 1; i++)
        {
            Gizmos.DrawLine
            (
                navPath.Nodes[i].GetExpandedPosition(agent.EnclosingCircleRadius),
                navPath.Nodes[i + 1].GetExpandedPosition(agent.EnclosingCircleRadius)
            );
            Gizmos.DrawWireSphere(navPath.Nodes[i].GetExpandedPosition(agent.EnclosingCircleRadius), 0.3f);
            Gizmos.DrawWireSphere(navPath.Nodes[i + 1].GetExpandedPosition(agent.EnclosingCircleRadius), 0.3f);
        }
    }
}
