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

    protected void Start()
    {
        agentTracker = Agent.GetComponents<NavGraphTracker>().FirstOrDefault(t => t.NavGraph.name == navGraphName);
    }

    public override bool Decompose(SteeringGoal goal)
    {
        if (!goal.HasPosition) return true;
        float agentRadius = Agent.PhysicsRadius;

        Vector2? nonBlockAgent = MathUtility.UnblockPosition(Agent.PhysicsCenter, agentRadius, agentTracker.NavGraph.WallMask);
        Vector2? nonBlockGoal = MathUtility.UnblockPosition(goal.Position, agentRadius, agentTracker.NavGraph.WallMask);

#if UNITY_EDITOR
        gizmoAgentPosition = nonBlockAgent;
        gizmoGoalPosition = nonBlockGoal;
#endif

        if (!nonBlockAgent.HasValue || !nonBlockGoal.HasValue) return false;
        goal.Position = nonBlockGoal.Value;

        if (IsGoalVisible(nonBlockAgent.Value, goal.Position, agentRadius)) return true;

        navPath = GetNavPath(goal);
        if (navPath == null) return false;

        goal.Position = GetMostDistantReachablePosition(nonBlockAgent.Value, agentRadius);
        return true;
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
            NavPath newNavPath = agentTracker.NavGraph.FindShortestPath(startNode, endNode);

            if (navPath != null && newNavPath != null && navPath.GetStart() != newNavPath.GetStart())
            {
                for (int i = 0; i < navPath.Nodes.Count; i++)
                {
                    for (int j = 1; j < newNavPath.Nodes.Count; j++)
                    {
                        if (navPath.Nodes[i] == newNavPath.Nodes[j])
                        {
                            var oldPathPart = navPath.Nodes.Take(i);
                            var newPathPart = newNavPath.Nodes.Skip(j);
                            newNavPath.Nodes = oldPathPart.Concat(newPathPart).ToList();
                            return newNavPath;
                        }
                    }
                }
            }

            return newNavPath;
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

    public override void Disable()
    {
        navPath = null;
    }

#if UNITY_EDITOR
    public override void DrawGizmos()
    {
        if (!Application.isPlaying || navPath == null || !gizmoAgentPosition.HasValue || !gizmoGoalPosition.HasValue) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoAgentPosition.Value, 0.3f);
        Gizmos.DrawWireSphere(gizmoGoalPosition.Value, 0.3f);
        for (int i = GetMostDistantReachableIndex(gizmoAgentPosition.Value, Agent.PhysicsRadius); i < navPath.Nodes.Count - 1; i++)
        {
            Gizmos.DrawLine
            (
                navPath.Nodes[i].GetExpandedPosition(Agent.PhysicsRadius),
                navPath.Nodes[i + 1].GetExpandedPosition(Agent.PhysicsRadius)
            );
            Gizmos.DrawWireSphere(navPath.Nodes[i].GetExpandedPosition(Agent.PhysicsRadius), 0.3f);
            Gizmos.DrawWireSphere(navPath.Nodes[i + 1].GetExpandedPosition(Agent.PhysicsRadius), 0.3f);
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
