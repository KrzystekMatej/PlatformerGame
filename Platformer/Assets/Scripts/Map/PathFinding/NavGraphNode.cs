using System;
using System.Collections.Generic;
using UnityEngine;
using HeapNode = FibonacciHeap.FibonacciHeapNode<NavGraphNode, float>;

public class NavGraphNode : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Ingoing;
    [HideInInspector]
    public Vector2 Outgoing;
    [HideInInspector]
    public int Index;
    [HideInInspector]
    public List<NavGraphNode> Neighbors = new List<NavGraphNode>();

private void Start()
    {
#if !UNITY_EDITOR
         Destroy(GetComponent<SpriteRenderer>());
#endif
    }

    public void AddDirectNeighbor(NavGraphNode neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
        }
    }


    public void RemoveDirectNeighbor(NavGraphNode neighbor)
    {
        Neighbors.Remove(neighbor);
    }

    public void AddUndirectNeighbor(NavGraphNode neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
            if (!neighbor.Neighbors.Contains(this))
            {
                neighbor.Neighbors.Add(this);
            }
        }
    }

    public void RemoveUndirectNeighbor(NavGraphNode neighbor)
    {
        if (Neighbors.Remove(neighbor)) neighbor.Neighbors.Remove(this);
    }

    public Vector2 GetExpandedPosition(float radius)
    {
        return transform.position + (Vector3)MathUtility.GetExpansionOffsetCC(Ingoing, Outgoing, radius);
    }
}