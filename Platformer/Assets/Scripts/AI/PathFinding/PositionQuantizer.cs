using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PositionQuantizer
{
    [field: SerializeField]
    public NavGraph NavGraph { get; set; }
    [field: SerializeField]
    public bool CoherenceEnabled { get; set; }
    [field: SerializeField]
    public CastDetector SolidGeometryDetector { get; private set; }

    public NavGraphNode CoherenceNode { get; private set; }

    private static readonly Func<NavGraph, NavGraphNode, NavGraphNode, Vector2, float> directDistance
        = (navGraph, node, reachableNode, distanceOrigin)
        => Vector2.Distance(distanceOrigin, node.transform.position);

    private static readonly Func<NavGraph, NavGraphNode, NavGraphNode, Vector2, float> graphDistance
        = (navGraph, node, goalNode, distanceOrigin)
        => navGraph.GetGraphDistance(node, goalNode) + Vector2.Distance(distanceOrigin, goalNode.transform.position);

    public NavGraphNode QuantizePositionToNode(Vector2 origin, NavGraphNode reachableNode = null)
     => QuantizePosition(origin, origin, reachableNode, directDistance);

    public NavGraphNode QuantizePositionToGoal(Vector2 agentPosition, Vector2 goalPosition, NavGraphNode goalNode)
     => QuantizePosition(agentPosition, goalPosition, goalNode, directDistance);

    public NavGraphNode QuantizePositionToShortestPath(Vector2 agentPosition, Vector2 goalPosition, NavGraphNode goalNode)
    => QuantizePosition(agentPosition, goalPosition, goalNode, graphDistance);


    private NavGraphNode QuantizePosition(Vector2 castOrigin, Vector2 distanceOrigin, NavGraphNode goalNode,
        Func<NavGraph, NavGraphNode, NavGraphNode, Vector2, float> distanceFunction)
    {
        NavGraphNode quantizedPosition = null;
        if (!CoherenceEnabled || CoherenceNode == null)
        {
            IEnumerable<NavGraphNode> nodesToCheck = goalNode != null ? NavGraph.TraverseDepthSearch(goalNode) : NavGraph.Nodes;
            quantizedPosition = QuantizePositionFromList(castOrigin, distanceOrigin, goalNode, distanceFunction, nodesToCheck);
        }
        else
        {
            CoherenceNode.Neighbors.Add(CoherenceNode);
            quantizedPosition = QuantizePositionFromList(castOrigin, distanceOrigin, goalNode, distanceFunction, CoherenceNode.Neighbors);
            CoherenceNode.Neighbors.RemoveAt(CoherenceNode.Neighbors.Count - 1);

        }
        if (CoherenceEnabled) CoherenceNode = quantizedPosition;
        return quantizedPosition;
    }

    private NavGraphNode QuantizePositionFromList(Vector2 castOrigin, Vector2 distanceOrigin, NavGraphNode goalNode,
        Func<NavGraph, NavGraphNode, NavGraphNode, Vector2, float> distanceFunction, IEnumerable<NavGraphNode> nodesToCheck)
    {
        NavGraphNode result = null;
        float minDistance = float.PositiveInfinity;

        foreach (NavGraphNode node in nodesToCheck)
        {
            SolidGeometryDetector.Direction = (Vector2)node.transform.position - castOrigin;
            SolidGeometryDetector.Distance = SolidGeometryDetector.Direction.magnitude;

            float distance = distanceFunction(NavGraph, node, goalNode, distanceOrigin);
            if (SolidGeometryDetector.Detect(castOrigin) == 0 && distance < minDistance)
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
