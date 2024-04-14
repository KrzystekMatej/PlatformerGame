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
    [SerializeField]
    private string navGraphName;
    private NavPath navPath;

    private NavGraphTracker agentTracker;
    private (Vector2 position, NavGraphNode node) cachedGoalInfo = (Vector2.negativeInfinity, null);
#if UNITY_EDITOR
    private Vector2? gizmoAgentPosition;
    private Vector2? gizmoGoalPosition;
#endif

    protected override void Start()
    {
        base.Start();
        agentTracker = agent.GetComponents<NavGraphTracker>().FirstOrDefault(t => t.NavGraph.name == navGraphName);
    }

    public override SteeringGoal Decompose(SteeringGoal goal)
    {
        if (!goal.HasPosition) return goal;
        float agentRadius = agent.EnclosingCircleRadius;

        Vector2? nonBlockAgent = MathUtility.UnblockPosition(agent.CenterPosition, agentRadius, agentTracker.NavGraph.WallMask);
        Vector2? nonBlockGoal = MathUtility.UnblockPosition(goal.Position, agentRadius, agentTracker.NavGraph.WallMask);

#if UNITY_EDITOR
        gizmoAgentPosition = nonBlockAgent;
        gizmoGoalPosition = nonBlockGoal;
#endif

        if (!nonBlockAgent.HasValue || !nonBlockGoal.HasValue) return new SteeringGoal();
        goal.Position = nonBlockGoal.Value;

        if (IsGoalVisible(nonBlockAgent.Value, goal.Position, agentRadius)) return goal;

        navPath = GetNavPath(goal);
        if (navPath == null) return new SteeringGoal();

        goal.Position = GetMostDistantReachablePosition(nonBlockAgent.Value, agentRadius);
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
        cachedGoalInfo = (goal.Position, endNode);

        
        if (startNode && endNode)
        {
            NavPath navPath = agentTracker.NavGraph.FindShortestPath(startNode, endNode);
            return navPath;
        }
        else return null;
    }

    private NavGraphNode QuantizeGoal(SteeringGoal goal)
    {
        if (goal.Position == cachedGoalInfo.position) return cachedGoalInfo.node;

        if (goal.HasOwner)
        {
            NavGraphTracker tracker = goal.Owner.GetComponents<NavGraphTracker>().FirstOrDefault(t => t.NavGraph == agentTracker.NavGraph);
            if (tracker && tracker.NavGraph == agentTracker.NavGraph) return tracker.Current;
        }

        return agentTracker.NavGraph.QuantizePosition(goal.Position);
    }

    private Vector2 GetMostDistantReachablePosition(Vector2 agentPosition, float enclosingCircleRadius)
    {
        for (int i = navPath.Nodes.Count - 1; i > 0; i--)
        {
            Vector2 expandedPosition = navPath.Nodes[i].GetExpandedPosition(enclosingCircleRadius);
            Vector2 nodeVector = expandedPosition - agentPosition;
            if (!Physics2D.CircleCast(agentPosition, enclosingCircleRadius, nodeVector, nodeVector.magnitude, agentTracker.NavGraph.WallMask))
            {
                return expandedPosition;
            }
        }

        return navPath.Nodes[0].GetExpandedPosition(enclosingCircleRadius);
    }

#if UNITY_EDITOR
    public override void DrawGizmos()
    {
        if (!Application.isPlaying || navPath == null || !gizmoAgentPosition.HasValue || !gizmoGoalPosition.HasValue) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoAgentPosition.Value, 0.3f);
        Gizmos.DrawWireSphere(gizmoGoalPosition.Value, 0.3f);
        for (int i = GetMostDistantReachableIndex(gizmoAgentPosition.Value, agent.EnclosingCircleRadius); i < navPath.Nodes.Count - 1; i++)
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

    private int GetMostDistantReachableIndex(Vector2 agentPosition, float enclosingCircleRadius)
    {
        for (int i = navPath.Nodes.Count - 1; i > 0; i--)
        {
            Vector2 expandedPosition = navPath.Nodes[i].GetExpandedPosition(enclosingCircleRadius);
            Vector2 nodeVector = expandedPosition - agentPosition;
            if (!Physics2D.CircleCast(agentPosition, enclosingCircleRadius, nodeVector, nodeVector.magnitude, agentTracker.NavGraph.WallMask))
            {
                return i;
            }
        }

        return 0;
    }
#endif
}
