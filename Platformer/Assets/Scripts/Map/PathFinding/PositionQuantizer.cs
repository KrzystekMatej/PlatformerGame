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

    public NavGraphNode CoherenceNode { get; private set; }


    public NavGraphNode QuantizePosition(Vector2 origin, NavGraphNode reachableNode = null)
    {
        NavGraphNode quantizedPosition;
        if (!CoherenceEnabled || CoherenceNode == null)
        {
            quantizedPosition = QuantizePositionFromList(origin, reachableNode != null ? NavGraph.TraverseDepthSearch(reachableNode) : NavGraph.Nodes);
        }
        else
        {
            CoherenceNode.Neighbors.Add(CoherenceNode);
            quantizedPosition = QuantizePositionFromList(origin, CoherenceNode.Neighbors);
            CoherenceNode.Neighbors.RemoveAt(CoherenceNode.Neighbors.Count - 1);

        }
        if (CoherenceEnabled) CoherenceNode = quantizedPosition;
        return quantizedPosition;
    }

    private NavGraphNode QuantizePositionFromList(Vector2 origin, IEnumerable<NavGraphNode> nodesToCheck)
    {
        NavGraphNode result = null;
        float minDistance = float.PositiveInfinity;

        foreach (NavGraphNode node in nodesToCheck)
        {
            Vector2 nodeVector = (Vector2)node.transform.position - origin;

            float distance = Vector2.Distance(origin, node.transform.position);
            if (distance < minDistance && !Physics2D.Raycast(origin, nodeVector, nodeVector.magnitude, NavGraph.SolidGeometryLayerMask))
            {
                minDistance = distance;
                result = node;
            }
        }

        return result;
    }
}