using FibonacciHeap;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NavGraph : MonoBehaviour
{
    public LayerMask WallMask;
    [SerializeField]
    private bool precalculationEnabled;
    public float CollisionAvoidanceMargin = 0.1f;
    public float MaxAllowedRadius = 0.85f;

#if UNITY_EDITOR
    public Sprite NodeSprite;
    public List<Collider2D> TraversableZones;
    public List<Collider2D> Walls;
    [HideInInspector]
    public GameObject NodeContainer;

    public bool ShowEdges;
    public NavPath TestPath;
#endif

    private class NodeSearchState
    {
        public uint SearchId;
        public FibonacciHeapNode<NavGraphNode, float> HeapNode;
        public float GCost;
        public float HCost;
        public bool Closed;
        public NavGraphNode From;
    }

    [HideInInspector]
    public List<NavGraphNode> Nodes = new List<NavGraphNode>();
    private List<NodeSearchState> searchStates;
    private uint searchCount;
    private (int nodeIndex, float length)[,] shortestPathTable;

    private void Awake()
    {
        searchStates = Enumerable.Range(0, Nodes.Count).Select(i => new NodeSearchState()).ToList();
        if (precalculationEnabled) PrecalculateShortestPaths();
    }

    private void AddNode(NavGraphNode node)
    {
        node.Index = Nodes.Count;
        Nodes.Add(node);
        searchStates.Add(new NodeSearchState());
    }

    private void RemoveNode(NavGraphNode nodeA)
    {

        foreach (NavGraphNode nodeB in Nodes)
        {
            nodeB.RemoveUndirectNeighbor(nodeA);
        }

        Nodes.RemoveAt(nodeA.Index);
        searchStates.RemoveAt(nodeA.Index);

        for (int i = nodeA.Index; i < Nodes.Count; i++)
        {
            Nodes[i].Index = i;
        }
    }

    public void ConnectNode(NavGraphNode node, float agentRadius, NavGraphNode coherenceNode = null)
    {
        AddEdges(node, agentRadius, coherenceNode);
        AddNode(node);
    }

    public void DisconnectNode(NavGraphNode node)
    {
        RemoveNode(node);
        node.Neighbors.Clear();
    }

    public void AddEdges(NavGraphNode nodeA, float radius, NavGraphNode coherenceNode = null)
    {
        List<NavGraphNode> candidates = coherenceNode ? coherenceNode.Neighbors : Nodes;

        foreach (NavGraphNode nodeB in candidates)
        {
            if (nodeA == nodeB) continue;
            Vector2 positionA = nodeA.GetExpandedPosition(radius);
            Vector2 positionB = nodeB.GetExpandedPosition(radius);
            Vector2 edgeVector = positionB - positionA;
            RaycastHit2D hit = Physics2D.CircleCast(positionA, radius, edgeVector, edgeVector.magnitude, WallMask);

            if (!hit) nodeA.AddUndirectNeighbor(nodeB);
        }
    }

    public void AddClosestEdges(NavGraphNode nodeA, float radius, int edgeCount)
    {
        List<(NavGraphNode node, float distance)> candidates = new List<(NavGraphNode, float)>();

        foreach (NavGraphNode nodeB in Nodes)
        {
            if (nodeA == nodeB) continue;
            Vector2 positionA = nodeA.GetExpandedPosition(radius);
            Vector2 positionB = nodeB.GetExpandedPosition(radius);
            Vector2 edgeVector = positionB - positionA;
            float distance = edgeVector.magnitude;
            RaycastHit2D hit = Physics2D.CircleCast(positionA, radius, edgeVector, distance, WallMask);

            if (!hit) candidates.Add((nodeB, distance));
        }

        candidates.Sort((a, b) => a.distance.CompareTo(b.distance));
        int selectedCount = Mathf.Min(candidates.Count, edgeCount);

        for (int i = 0; i < selectedCount; i++)
        {
            nodeA.AddUndirectNeighbor(candidates[i].node);
        }
    }


    public NavPath DijkstraShortestPath(NavGraphNode start, Func<NavGraphNode, bool> terminationCondition)
    {
        return FindShortestPath
        (
            start,
            terminationCondition,
            (current) => 0
        );
    }

    public NavPath FindShortestPath(NavGraphNode start, NavGraphNode end)
    {
        if (precalculationEnabled) return GetPrecalculatedPath(start, end);
        return AStarShortestPath(start, end);
    }

    public NavPath AStarShortestPath(NavGraphNode start, NavGraphNode end)
    {
        return FindShortestPath
        (
            start,
            (current) => current == end,
            (current) => Vector2.Distance(current.transform.position, end.transform.position)
        );
    }

    public NavPath FindShortestPath(NavGraphNode start, Func<NavGraphNode, bool> terminationCondition, Func<NavGraphNode, float> heuristic)
    {
        IncreaseSearchId();

        FibonacciHeap<NavGraphNode, float> queue = new FibonacciHeap<NavGraphNode, float>(float.NegativeInfinity);

        NodeSearchState startState = searchStates[start.Index];
        startState.SearchId = searchCount;
        startState.GCost = 0;
        startState.HCost = heuristic(start);
        startState.HeapNode = new FibonacciHeapNode<NavGraphNode, float>(start, startState.HCost);
        startState.Closed = false;
        startState.From = null;
        queue.Insert(searchStates[start.Index].HeapNode);

        while (!queue.IsEmpty())
        {
            NavGraphNode current = queue.Min().Data;
            if (terminationCondition(current)) return ReconstructPath(current);

            NodeSearchState currentState = searchStates[current.Index];

            foreach (NavGraphNode neighbor in current.Neighbors)
            {
                float neighborGCost = currentState.GCost + Vector2.Distance(current.transform.position, neighbor.transform.position);
                NodeSearchState neighborState = searchStates[neighbor.Index];

                if (neighborState.SearchId == searchCount)
                {
                    if (neighborState.Closed || neighborGCost >= neighborState.GCost) continue;
                    else queue.DecreaseKey(neighborState.HeapNode, neighborGCost + neighborState.HCost);
                }
                else
                {
                    neighborState.SearchId = searchCount;
                    neighborState.HCost = heuristic(neighbor);
                    neighborState.Closed = false;
                    neighborState.HeapNode = new FibonacciHeapNode<NavGraphNode, float>(neighbor, neighborGCost + neighborState.HCost);
                    queue.Insert(neighborState.HeapNode);
                }

                neighborState.GCost = neighborGCost;
                neighborState.From = current;
            }

            queue.RemoveMin();
            currentState.Closed = true;
        }

        return null;
    }

    private void IncreaseSearchId()
    {
        if (searchCount == uint.MaxValue)
        {
            ResetSearchData();
            searchCount++;
        }
        searchCount++;
    }

    private void ResetSearchData()
    {
        foreach (NodeSearchState state in searchStates)
        {
            state.SearchId = 0;
            state.HeapNode = null;
            state.Closed = false;
            state.From = null;
        }
    }

    public NavPath GetPrecalculatedPath(NavGraphNode start, NavGraphNode end)
    {
        if (!precalculationEnabled)
            throw new InvalidOperationException("Precalculation must be enabled to reconstruct a path.");

        if (shortestPathTable[start.Index, end.Index].nodeIndex == -1) return null;

        List<NavGraphNode> path = new List<NavGraphNode>();
        int current = start.Index;

        while (current != end.Index)
        {
            path.Add(Nodes[current]);
            current = shortestPathTable[current, end.Index].nodeIndex;
        }
        path.Add(Nodes[current]);

        return new NavPath(path, shortestPathTable[start.Index, end.Index].length);
    }

    public float GetGraphDistance(NavGraphNode start, NavGraphNode end)
    {
        if (precalculationEnabled) return shortestPathTable[start.Index, end.Index].length;
        return AStarShortestPath(start, end).Length;
    }

    private NavPath ReconstructPath(NavGraphNode end)
    {
        List<NavGraphNode> path = new List<NavGraphNode>();

        NavGraphNode current = end;
        while (current)
        {
            path.Add(current);
            current = searchStates[current.Index].From;
        }

        path.Reverse();
        return new NavPath(path, searchStates[end.Index].GCost);
    }

    public static float GetPathSegmentLength(List<NavGraphNode> path, int start, int count)
    {
        float length = 0;
        for (int i = start; i < count - 1; i++)
        {
            length += Vector3.Distance(path[i].transform.position, path[i + 1].transform.position);
        }
        return length;
    }

    public void PrecalculateShortestPaths()
    {
        shortestPathTable = new (int, float)[Nodes.Count, Nodes.Count];


        for (int i = 0; i < Nodes.Count; i++)
        {
            for (int j = 0; j < Nodes.Count; j++)
            {
                shortestPathTable[i, j] = (-1, -1f);
            }
        }


        for (int source = 0; source < Nodes.Count; source++)
        {
            DijkstraShortestPath(Nodes[source], (current) => false);

            for (int target = 0; target < Nodes.Count; target++)
            {
                if (source == target) shortestPathTable[source, target] = (target, 0);
                else
                {
                    int current = target;
                    NodeSearchState currentState = searchStates[current];

                    while (current != source && currentState.From && searchStates[currentState.From.Index].SearchId == searchCount)
                    {
                        float length = currentState.GCost + Vector2.Distance(Nodes[current].transform.position, Nodes[target].transform.position);
                        shortestPathTable[currentState.From.Index, target] = (current, length);
                        current = currentState.From.Index;
                        currentState = searchStates[current];
                    }
                }
            }
        }
    }

    public IEnumerable<NavGraphNode> TraverseDepthSearch(NavGraphNode start)
    {
        IncreaseSearchId();
        Stack<NavGraphNode> stack = new Stack<NavGraphNode>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            NavGraphNode current = stack.Pop();
            searchStates[current.Index].SearchId = searchCount;

            yield return current;

            foreach (NavGraphNode node in current.Neighbors)
            {
                if (searchStates[node.Index].SearchId != searchCount) stack.Push(node);
            }
        }
    }

    public NavGraphNode QuantizePosition(Vector2 origin, NavGraphNode coherenceNode = null)
    {
        if (coherenceNode)
        {
            coherenceNode.Neighbors.Add(coherenceNode);
            NavGraphNode quantizedPosition = QuantizePositionFromList(origin, coherenceNode.Neighbors);
            coherenceNode.Neighbors.RemoveAt(coherenceNode.Neighbors.Count - 1);
            return quantizedPosition;
        }
        return QuantizePositionFromList(origin, Nodes);
    }

    private NavGraphNode QuantizePositionFromList(Vector2 origin, IEnumerable<NavGraphNode> nodesToCheck)
    {
        NavGraphNode result = null;
        float minDistance = float.PositiveInfinity;

        foreach (NavGraphNode node in nodesToCheck)
        {
            Vector2 nodeVector = (Vector2)node.transform.position - origin;

            float distance = Vector2.Distance(origin, node.transform.position);
            if (distance < minDistance && !Physics2D.Raycast(origin, nodeVector, nodeVector.magnitude, WallMask))
            {
                minDistance = distance;
                result = node;
            }
        }

        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!ShowEdges) return;
        if (Nodes == null) return;
        foreach (NavGraphNode node in Nodes)
        {
            Gizmos.color = Color.cyan;
            if (!node || node.Neighbors == null) continue;
            foreach (NavGraphNode neighbor in node.Neighbors)
            {
                Gizmos.DrawLine(node.transform.position, neighbor.transform.position);
            }
        }

        if (TestPath != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < TestPath.Nodes.Count - 1; i++)
            {
                Gizmos.DrawLine(TestPath.Nodes[i].transform.position, TestPath.Nodes[i + 1].transform.position);
            }
        }
    }

    public void InitializeSearchData()
    {
        searchStates = Enumerable.Range(0, Nodes.Count).Select(i => new NodeSearchState()).ToList();
    }
#endif
}