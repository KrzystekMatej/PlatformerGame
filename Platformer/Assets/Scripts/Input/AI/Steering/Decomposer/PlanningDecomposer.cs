using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlanningDecomposer : Decomposer
{
    [SerializeField]
    private Path path;
    private NavPath navPath;

    private ArriveTargeter arriveTargeter;
    private NavGraphTracker agentTracker;

    private void Awake()
    {
        agentTracker = GetComponentInParent<AIInputController>().GetComponentInChildren<NavGraphTracker>();
        arriveTargeter = GetComponent<ArriveTargeter>();
    }

    public override SteeringGoal Decompose(AgentManager agent, SteeringGoal goal)
    {
        if (!goal.HasPosition) return goal;

        if (IsGoalVisible(agent.CenterPosition, arriveTargeter.ArrivePosition, agent.EnclosingCircleRadius)) return goal;

        navPath = GetNavPath(agent, goal);
        path.Points.Clear();
        if (navPath == null) return new SteeringGoal();

        RecalculatePath(agent.CenterPosition, goal.Position, agent.EnclosingCircleRadius);

        goal.Position = path.CalculateGoalWithCoherence(agent);
    
        return goal;
    }

    private bool IsGoalVisible(Vector2 agentPosition, Vector2 goalPosition, float enclosingCircleRadius)
    {
        Vector2 goalVector = goalPosition - agentPosition;
        return !Physics2D.CircleCast(agentPosition, enclosingCircleRadius, goalVector, goalVector.magnitude, agentTracker.NavGraph.WallMask);
    }

    private NavPath GetNavPath(AgentManager agent, SteeringGoal goal)
    {
        NavGraphNode start = agentTracker.Current;
        NavGraphNode end = QuantizeGoal(goal);
        
        if (start && end)
        {
            if (navPath != null && navPath.GetStart() == start && navPath.GetEnd() == end) return navPath;
            else return agentTracker.NavGraph.FindShortestPath(start, end);
        }
        else return null;
    }

    private NavGraphNode QuantizeGoal(SteeringGoal goal)
    {
        if (path.Points.Count > 0 && goal.Position == path.Points[path.Points.Count-1]) return navPath.GetEnd();

        if (goal.HasOwner)
        {
            NavGraphTracker tracker = goal.Owner.GetComponents<NavGraphTracker>().FirstOrDefault(t => t.NavGraph == agentTracker.NavGraph);
            if (tracker && tracker.NavGraph == agentTracker.NavGraph) return tracker.Current;
        }

        return agentTracker.NavGraph.QuantizePosition(goal.Position);
    }

    private void RecalculatePath(Vector2 agentPosition, Vector2 goalPosition, float enclosingCircleRadius)
    {
        int distant = GetMostDistantReachableGoal(agentPosition, enclosingCircleRadius);
        int closest = GetClosestReachableNode(goalPosition, enclosingCircleRadius);

        if (distant <= closest)
        {
            path.Points.AddRange(navPath.GetExpandedPositions(enclosingCircleRadius, distant, closest + 1));
        }
        else
        {
            path.Points.AddRange(navPath.GetExpandedPositions(enclosingCircleRadius, distant, navPath.Nodes.Count));
        }


        path.Points.Add(goalPosition);
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

    private int GetClosestReachableNode(Vector2 arrivePosition, float enclosingCircleRadius)
    {
        for (int i = 0; i < navPath.Nodes.Count - 1; i++)
        {
            Vector2 nodeVector = navPath.Nodes[i].GetExpandedPosition(enclosingCircleRadius) - arrivePosition;
            if (!Physics2D.CircleCast(arrivePosition, enclosingCircleRadius, nodeVector, nodeVector.magnitude, agentTracker.NavGraph.WallMask))
            {
                return i;
            }
        }

        return navPath.Nodes.Count - 1;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        path.DrawAllGizmos();
    }
}
