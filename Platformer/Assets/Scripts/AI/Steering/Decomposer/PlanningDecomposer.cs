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
    private bool coherenceEnabled;

    private NavGraph navGraph;
    private NavGraphNode current;
    private NavPath navPath;

#if UNITY_EDITOR
    private Vector2 gizmoGoalPosition;
#endif

    private void Start()
    {
        navGraph = FindObjectOfType<NavGraph>();
    }

    public override SteeringGoal Decompose(Agent agent, SteeringGoal goal)
    {
#if UNITY_EDITOR
        gizmoGoalPosition = goal.Position;
#endif

        if (!goal.HasPosition) return goal;

        path.Points.Clear();

        NavGraphNode goalNode = QuantizeGoal(goal);
        NavGraphNode agentNode = navGraph.QuantizePosition(agent.CenterPosition, current, goalNode);
        current = coherenceEnabled ? agentNode : null;

        navPath = CalculateNavPath(agentNode, goalNode);
        if (navPath == null) return new SteeringGoal();

        path.Points.AddRange(navPath.GetExpandedPositions(agent.EnclosingCircleRadius, GetReachableNodeIndex1(agent.CenterPosition), GetReachableNodeIndex2(goal.Position) + 1));
        path.Points.Add(goal.Position);

        goal.Position = path.CalculateGoal(agent);
        return goal;
    }

    private NavPath CalculateNavPath(NavGraphNode start, NavGraphNode end)
    {
        if (start == null || end == null) return null;
        if (navPath != null && navPath.GetStart() == start && navPath.GetEnd() == end) return navPath;
        return navGraph.FindShortestPath(start, end);
    }

    private bool IsGoalVisible(Agent agent, SteeringGoal goal)
    {
        Vector2 goalVector = goal.Position - agent.CenterPosition;
        return !agent.CastCheck(goalVector, goalVector.magnitude, navGraph.SolidGeometryLayerMask);
    }

    private NavGraphNode QuantizeGoal(SteeringGoal goal)
    {
        if (goal.HasOwner)
        {
            NavGraphTracker goalTracker = goal.Owner.GetComponent<NavGraphTracker>();
            if (goalTracker != null) return goalTracker.Current;
        }

        return navGraph.QuantizePosition(goal.Position, null, current);
    }

    public int GetReachableNodeIndex1(Vector2 origin)
    {
        int nodeIndex = 0;

        for (int i = 0; i < navPath.Nodes.Count; i++)
        {
            Vector2 nodeVector = (Vector2)navPath.Nodes[i].transform.position - origin;

            if (!Physics2D.Raycast(origin, nodeVector, nodeVector.magnitude, navGraph.SolidGeometryLayerMask))
            {
                nodeIndex = i;
                break;
            }
        }

        return nodeIndex;
    }


    public int GetReachableNodeIndex2(Vector2 origin)
    {
        int nodeIndex = 0;

        for (int i = navPath.Nodes.Count - 1; i > 0; i--)
        {
            Vector2 nodeVector = (Vector2)navPath.Nodes[i].transform.position - origin;

            if (!Physics2D.Raycast(origin, nodeVector, nodeVector.magnitude, navGraph.SolidGeometryLayerMask))
            {
                nodeIndex = i;
                break;
            }
        }

        return nodeIndex;
    }


    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        path.DrawGizmos();
    }
}
