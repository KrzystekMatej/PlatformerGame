using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionQuantizer
{
    private NavGraph navGraph;
    private QuantizationType quantizationType;
    private NavGraphNode coherenceNode;
    private Func<NavGraphNode, bool> castFunction;
    private Func<NavGraphNode, float> distanceFunction;

    private Func<NavGraphNode, bool> GetAgentCastFunction(Agent agent)
    {

        return (node) =>
        {
            Vector2 nodeVector = node.GetExpandedPosition(agent.EnclosingCircleRadius) - agent.CenterPosition;
            return agent.CastCheck(nodeVector, nodeVector.magnitude, navGraph.SolidGeometryLayerMask);
        };
    }

    public NavGraphNode QuantizePosition(NavGraphNode reachableNode = null)
    {

        if (coherenceNode == null) return QuantizePositionFromList(reachableNode != null ? navGraph.TraverseDepthSearch(reachableNode) : navGraph.Nodes);

        coherenceNode.Neighbors.Add(coherenceNode);
        NavGraphNode result = QuantizePositionFromList(coherenceNode.Neighbors);
        coherenceNode.Neighbors.RemoveAt(coherenceNode.Neighbors.Count - 1);
        return result;
    }

    private NavGraphNode QuantizePositionFromList(IEnumerable<NavGraphNode> nodesToCheck)
    {
        NavGraphNode result = null;
        float minDistance = float.PositiveInfinity;

        foreach (NavGraphNode node in nodesToCheck)
        {
            bool collision = castFunction(node);

            float distance = distanceFunction(node);
            if (!collision && distance < minDistance)
            {
                minDistance = distance;
                result = node;
            }
        }

        return result;
    }
}

public enum QuantizationType
{
    ToNode,
    ToGoal,
    ToShortestPath
}
